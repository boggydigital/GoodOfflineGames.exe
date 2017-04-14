﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

using Interfaces.FileDownload;
using Interfaces.Extraction;
using Interfaces.Routing;
using Interfaces.Status;
using Interfaces.Network;
using Interfaces.Expectation;

namespace GOG.Controllers.FileDownload
{
    public class ManualUrlDownloadFromSourceDelegate : IDownloadFileFromSourceDelegate
    {
        private INetworkController networkController;
        private IStringExtractionController uriSansSessionExtractionController;
        private IRoutingController routingController;
        private IFileDownloadController fileDownloadController;
        private IStatusController statusController;
        private IDownloadFileFromSourceDelegate validationDownloadFileFromSourceDelegate;

        public ManualUrlDownloadFromSourceDelegate(
            INetworkController networkController,
            IStringExtractionController uriSansSessionExtractionController,
            IRoutingController routingController,
            IFileDownloadController fileDownloadController,
            IDownloadFileFromSourceDelegate validationDownloadFileFromSourceDelegate,
            IStatusController statusController)
        {
            this.networkController = networkController;
            this.uriSansSessionExtractionController = uriSansSessionExtractionController;
            this.routingController = routingController;
            this.fileDownloadController = fileDownloadController;
            this.validationDownloadFileFromSourceDelegate = validationDownloadFileFromSourceDelegate;
            this.statusController = statusController;
        }

        public async Task DownloadFileFromSourceAsync(long id, string title, string sourceUri, string destination, IStatus status)
        {
            var downloadTask = statusController.Create(status, "Download game details manual url");

            HttpResponseMessage response;
            try
            {
                response = await networkController.RequestResponse(downloadTask, HttpMethod.Get, sourceUri);
            }
            catch (HttpRequestException ex)
            {
                statusController.Fail(
                    downloadTask,
                    $"Failed to get successful response for {sourceUri} for " +
                    $"product {id}: {title}, message: {ex.Message}");
                statusController.Complete(downloadTask);
                return;
            }

            using (response)
            {

                var resolvedUri = response.RequestMessage.RequestUri.ToString();

                // GOG.com quirk
                // When resolving ManualUrl from GameDetails we get CDN Uri with the session key.
                // Storing this key is pointless - it expries after some time and needs to be updated.
                // So here we filter our this session key and store direct file Uri

                var uriSansSession = uriSansSessionExtractionController.ExtractMultiple(resolvedUri).Single();

                await routingController.UpdateRouteAsync(
                    id,
                    title,
                    sourceUri,
                    uriSansSession,
                    downloadTask);

                try
                {
                    await fileDownloadController.DownloadFileFromResponseAsync(
                        response, 
                        destination, 
                        downloadTask);
                }
                catch (Exception ex)
                {
                    statusController.Fail(
                        downloadTask, 
                        $"Couldn't download {sourceUri}, resolved as {resolvedUri} to {destination} " +
                        $"for product {id}: {title}, error message: {ex.Message}");
                }

                // GOG.com quirk
                // Supplementary download is a secondary download to a primary driven by download scheduling
                // The example is validation file - while we can use the same pipeline, we would be
                // largerly duplicating all the work to establish the session, compute the name etc.
                // While the only difference validation files have - is additional extension.
                // So instead we'll do a supplementary download using primary download information

                validationDownloadFileFromSourceDelegate?.DownloadFileFromSourceAsync(
                    id, 
                    title, 
                    resolvedUri,
                    destination, 
                    downloadTask);
            }

            statusController.Complete(downloadTask);
        }
    }
}