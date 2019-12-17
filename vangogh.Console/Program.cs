﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Delegates.Convert;
using Delegates.Convert.Requests;
using Delegates.GetFilename;
using Delegates.Format.Uri;
using Delegates.Itemize;
using Delegates.Confirm;
using Delegates.Recycle;
using Delegates.Correct;
using Delegates.Replace;
using Delegates.Download;
using Delegates.Convert.Hashes;
using Delegates.Confirm.ArgsTokens;
using Delegates.GetDirectory.Root;
using Delegates.GetDirectory.ProductTypes;
using Delegates.GetPath.Json;
using Delegates.Itemize.Attributes;
using Delegates.GetValue.Uri.ProductTypes;
using Delegates.GetValue.QueryParameters.ProductTypes;

using Controllers.Storage;
using Controllers.File;
using Controllers.Directory;
using Controllers.Uri;
using Controllers.Network;
using Controllers.Language;
using Controllers.Serialization.JSON;
using Controllers.Collection;
using Controllers.Validation;
using Controllers.ValidationResult;
using Controllers.Stash.ArgsDefinitions;
using Controllers.SerializedStorage.ProtoBuf;
using Controllers.Presentation;
using Controllers.Routing;
using Controllers.Status;
using Controllers.NotifyViewUpdate;
using Controllers.InputOutput;
using Controllers.Instances;
using Controllers.Records.Session;
using Controllers.Data.ProductTypes;
using Controllers.Stream;

using Interfaces.Delegates.Itemize;

using Interfaces.Models.Entities;

using GOG.Models;

using GOG.Controllers.Data.ProductTypes;

using GOG.Delegates.GetPageResults;
using GOG.Delegates.GetPageResults.ProductTypes;
using GOG.Delegates.FillGaps;
using GOG.Delegates.GetDownloadSources;
using GOG.Delegates.GetDownloadSources.ProductTypes;
using GOG.Delegates.DownloadProductFile;
using GOG.Delegates.GetImageUri;
using GOG.Delegates.UpdateScreenshots;
using GOG.Delegates.GetDeserialized;
using GOG.Delegates.GetDeserialized.ProductTypes;
using GOG.Delegates.Itemize;
using GOG.Delegates.Itemize.ProductTypes;
using GOG.Delegates.Convert.ProductTypes;
using GOG.Delegates.RequestPage;
using GOG.Delegates.Confirm;
using GOG.Delegates.Confirm.ProductTypes;
using GOG.Delegates.Convert.UpdateIdentity;

using GOG.Controllers.Authorization;

using GOG.Activities.Help;
using GOG.Activities.Authorize;
using GOG.Activities.Update;
using GOG.Activities.Update.ProductTypes;
using GOG.Activities.UpdateDownloads.ProductTypes;
using GOG.Activities.DownloadProductFiles;
using GOG.Activities.DownloadProductFiles.ProductTypes;
using GOG.Activities.Cleanup;
using GOG.Activities.Validate;
using GOG.Activities.Report;

using Models.Status;
using Models.QueryParameters;
using Models.Records;

using Delegates.GetPath.ArgsDefinitions;

namespace vangogh.Console
{
    class Program
    {
        static void PrintDependencies(params object[] items)
        {
            var knownAssemblies = new string[] {
                "Delegates",
                "Controllers",
                "GOG.Delegates",
                "GOG.Controllers"};

            System.Console.WriteLine("\t\t[Dependencies(");
            for (var ii=0;ii<items.Length;ii++)
            {
                var item = items[ii];
                var assembly = string.Empty;
                var type = item.GetType().ToString();
                foreach (var a in knownAssemblies)
                    if (type.StartsWith(a))
                    {
                        assembly = a;
                        break;
                    }

                if (string.IsNullOrEmpty(assembly))
                    throw new Exception();

                if (ii != items.Length - 1) System.Console.WriteLine($"\t\t\t\"{type},{assembly}\",");
                else System.Console.Write($"\t\t\t\"{type},{assembly}\"");
            }
            System.Console.WriteLine(")]");
        }

        static async Task Main(string[] args)
        {
            var singletonInstancesController = new SingletonInstancesController();

            // var getEmptyDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetEmptyDirectoryDelegate))
            //     as GetEmptyDirectoryDelegate;

            // var getTemplatesDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetTemplatesDirectoryDelegate))
            //     as GetTemplatesDirectoryDelegate;

            // var getDataDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetDataDirectoryDelegate))
            //     as GetDataDirectoryDelegate;

            // var getRecycleBinDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetRecycleBinDirectoryDelegate))
            //     as GetRecycleBinDirectoryDelegate;

            var getProductImagesDirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetProductImagesDirectoryDelegate))
                as GetProductImagesDirectoryDelegate;

            var getAccountProductImagesDirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetAccountProductImagesDirectoryDelegate))
                as GetAccountProductImagesDirectoryDelegate;

            var getReportDirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetReportDirectoryDelegate))
                as GetReportDirectoryDelegate;

            var getMd5DirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetMd5DirectoryDelegate))
                as GetMd5DirectoryDelegate;

            var getProductFilesRootDirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetProductFilesRootDirectoryDelegate))
                as GetProductFilesRootDirectoryDelegate;

            var getScreenshotsDirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetScreenshotsDirectoryDelegate))
                as GetScreenshotsDirectoryDelegate;

            var getProductFilesDirectoryDelegate = singletonInstancesController.GetInstance(
                typeof(GetProductFilesDirectoryDelegate))
                as GetProductFilesDirectoryDelegate;

            // var getArgsDefinitionsFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetArgsDefinitionsFilenameDelegate))
            //     as GetArgsDefinitionsFilenameDelegate;

            // var getHashesFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetHashesFilenameDelegate))
            //     as GetHashesFilenameDelegate;

            // var getAppTemplateFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetAppTemplateFilenameDelegate))
            //     as GetAppTemplateFilenameDelegate;

            // var getReportTemplateFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetReportTemplateFilenameDelegate))
            //     as GetReportTemplateFilenameDelegate;                

            // var getCookiesFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetCookiesFilenameDelegate))
            //     as GetCookiesFilenameDelegate;  

            // var getIndexFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetIndexFilenameDelegate))
            //     as GetIndexFilenameDelegate;

            // var getWishlistedFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetWishlistedFilenameDelegate))
            //     as GetWishlistedFilenameDelegate;

            // var getUpdatedFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetUpdatedFilenameDelegate))
            //     as GetUpdatedFilenameDelegate;

            var getUriFilenameDelegate = singletonInstancesController.GetInstance(
                typeof(GetUriFilenameDelegate))
                as GetUriFilenameDelegate;

            var getReportFilenameDelegate = singletonInstancesController.GetInstance(
                typeof(GetReportFilenameDelegate))
                as GetReportFilenameDelegate;

            var getValidationFilenameDelegate = singletonInstancesController.GetInstance(
                typeof(GetValidationFilenameDelegate))
                as GetValidationFilenameDelegate;

            // var getArgsDefinitionsPathDelegate = dependenciesController.GetInstance(
            //     typeof(GetArgsDefinitionsPathDelegate))
            //     as GetArgsDefinitionsPathDelegate;

            // var getHashesPathDelegate = dependenciesController.GetInstance(
            //     typeof(GetHashesPathDelegate))
            //     as GetHashesPathDelegate;

            // var getAppTemplatePathDelegate = dependenciesController.GetInstance(
            //     typeof(GetAppTemplatePathDelegate))
            //     as GetAppTemplatePathDelegate;

            // var getReportTemplatePathDelegate = dependenciesController.GetInstance(
            //     typeof(GetReportTemplatePathDelegate))
            //     as GetReportTemplatePathDelegate;

            // var getCookiePathDelegate = dependenciesController.GetInstance(
            //     typeof(GetCookiesPathDelegate))
            //     as GetCookiesPathDelegate;

            var getGameDetailsFilesPathDelegate = singletonInstancesController.GetInstance(
                typeof(GetGameDetailsFilesPathDelegate))
                as GetGameDetailsFilesPathDelegate;

            var getValidationPathDelegate = singletonInstancesController.GetInstance(
                typeof(GetValidationPathDelegate))
                as GetValidationPathDelegate;

            var statusController = singletonInstancesController.GetInstance(
                typeof(StatusController))
                as StatusController;

            var streamController = singletonInstancesController.GetInstance(
                typeof(StreamController))
                as StreamController;

            var fileController = singletonInstancesController.GetInstance(
                typeof(FileController))
                as FileController;

            var directoryController = singletonInstancesController.GetInstance(
                typeof(DirectoryController))
                as DirectoryController;

            var storageController = singletonInstancesController.GetInstance(
                typeof(StorageController))
                as StorageController;

            var jsonSerializationController = singletonInstancesController.GetInstance(
                typeof(JSONSerializationController))
                as JSONSerializationController;

            // var convertBytesToStringDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertBytesToStringDelegate))
            //     as ConvertBytesToStringDelegate;

            var convertBytesToMd5HashDelegate = singletonInstancesController.GetInstance(
                typeof(ConvertBytesToMd5HashDelegate))
                as ConvertBytesToMd5HashDelegate;

            // var convertStringToBytesDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertStringToBytesDelegate))
            //     as ConvertStringToBytesDelegate;

            var convertStringToMd5HashDelegate = singletonInstancesController.GetInstance(
                typeof(ConvertStringToMd5HashDelegate))
                as ConvertStringToMd5HashDelegate;

            // var protoBufSerializedStorageController = dependenciesController.GetInstance(
            //     typeof(ProtoBufSerializedStorageController))
            //     as ProtoBufSerializedStorageController;

            // var jsonSerializedStorageController = dependenciesController.GetInstance(
            //     typeof(JSONSerializedStorageController))
            //     as JSONSerializedStorageController;

            var argsDefinitionStashController = singletonInstancesController.GetInstance(
                typeof(ArgsDefinitionsStashController))
                as ArgsDefinitionsStashController;

            // var appTemplateStashController = dependenciesController.GetInstance(
            //     typeof(AppTemplateStashController))
            //     as AppTemplateStashController;

            // var reportTemplateStashController = dependenciesController.GetInstance(
            //     typeof(ReportTemplateStashController))
            //     as ReportTemplateStashController;

            // var cookiesStashController = dependenciesController.GetInstance(
            //     typeof(CookiesStashController))
            //     as CookiesStashController;

            // var consoleController = dependenciesController.GetInstance(
            //     typeof(ConsoleController))
            //     as ConsoleController;

            // var formatTextToFitConsoleWindowDelegate = dependenciesController.GetInstance(
            //     typeof(FormatTextToFitConsoleWindowDelegate))
            //     as FormatTextToFitConsoleWindowDelegate;

            var consoleInputOutputController = singletonInstancesController.GetInstance(
                typeof(ConsoleInputOutputController))
                as ConsoleInputOutputController;

            // var formatBytesDelegate = dependenciesController.GetInstance(
            //     typeof(FormatBytesDelegate))
            //     as FormatBytesDelegate;

            // var formatSecondsDelegate = dependenciesController.GetInstance(
            //     typeof(FormatSecondsDelegate))
            //     as FormatSecondsDelegate;

            var collectionController = singletonInstancesController.GetInstance(
                typeof(CollectionController))
                as CollectionController;

            // var itemizeStatusChildrenDelegate = dependenciesController.GetInstance(
            //     typeof(ItemizeStatusChildrenDelegate))
            //     as ItemizeStatusChildrenDelegate;

            // var convertStatusTreeToEnumerableDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertStatusTreeToEnumerableDelegate))
            //     as ConvertStatusTreeToEnumerableDelegate;

            var applicationStatus = singletonInstancesController.GetInstance(
                typeof(Status))
                as Status;

            // var appTemplateController = dependenciesController.GetInstance(
            //     typeof(AppTemplateController))
            //     as AppTemplateController;

            // var reportTemplateController = dependenciesController.GetInstance(
            //     typeof(ReportTemplateController))
            //     as ReportTemplateController;

            // var formatRemainingTimeAtSpeedDelegate = dependenciesController.GetInstance(
            //     typeof(FormatRemainingTimeAtSpeedDelegate))
            //     as FormatRemainingTimeAtSpeedDelegate;

            // var getStatusAppViewModelDelegate = dependenciesController.GetInstance(
            //     typeof(GetStatusAppViewModelDelegate))
            //     as GetStatusAppViewModelDelegate;

            // var statusReportViewModelDelegate = dependenciesController.GetInstance(
            //     typeof(GetStatusReportViewModelDelegate))
            //     as GetStatusReportViewModelDelegate;

            var getStatusViewUpdateDelegate = singletonInstancesController.GetInstance(
                typeof(GetStatusViewUpdateDelegate))
                as GetStatusViewUpdateDelegate;

            // var consoleNotifyStatusViewUpdateController = dependenciesController.GetInstance(
            //     typeof(NotifyStatusViewUpdateController))
            //     as NotifyStatusViewUpdateController;

            // TODO: Implement a better way
            // add notification handler to drive console view updates
            // statusController.NotifyStatusChangedAsync += consoleNotifyStatusViewUpdateController.NotifyViewUpdateOutputOnRefreshAsync;

            // var constrainExecutionAsyncDelegate = dependenciesController.GetInstance(
            //     typeof(ConstrainExecutionAsyncDelegate))
            //     as ConstrainExecutionAsyncDelegate;

            // var itemizeAllRateConstrainedUrisDelegate = dependenciesController.GetInstance(
            //     typeof(ItemizeAllRateConstrainedUrisDelegate))
            //     as ItemizeAllRateConstrainedUrisDelegate;

            // var constrainRequestRateAsyncDelegate = dependenciesController.GetInstance(
            //     typeof(ConstrainRequestRateAsyncDelegate))
            //     as ConstrainRequestRateAsyncDelegate;;

            var uriController = singletonInstancesController.GetInstance(
                typeof(UriController))
                as UriController;

            // var cookiesSerializationController = dependenciesController.GetInstance(
            //     typeof(CookiesSerializationController))
            //     as CookiesSerializationController;

            // var cookiesController = dependenciesController.GetInstance(
            //     typeof(CookiesController))
            //     as CookiesController;

            var networkController = singletonInstancesController.GetInstance(
                typeof(NetworkController))
                as NetworkController;

            var downloadFromResponseAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(DownloadFromResponseAsyncDelegate))
                as DownloadFromResponseAsyncDelegate;

            var downloadFromUriAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(DownloadFromUriAsyncDelegate))
                as DownloadFromUriAsyncDelegate;

            var requestPageAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(RequestPageAsyncDelegate))
                as RequestPageAsyncDelegate;

            var languageController = singletonInstancesController.GetInstance(
                typeof(LanguageController))
                as LanguageController;

            var itemizeGOGDataDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeGOGDataDelegate))
                as ItemizeGOGDataDelegate;

            var itemizeScreenshotsDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeScreenshotsDelegate))
                as ItemizeScreenshotsDelegate; ;

            var formatImagesUriDelegate = singletonInstancesController.GetInstance(
                typeof(FormatImagesUriDelegate))
                as FormatImagesUriDelegate;

            var formatScreenshotsUriDelegate = singletonInstancesController.GetInstance(
                typeof(FormatScreenshotsUriDelegate))
                as FormatScreenshotsUriDelegate;

            var recycleDelegate = singletonInstancesController.GetInstance(
                typeof(RecycleDelegate))
                as RecycleDelegate;

            var wishlistedDataController = singletonInstancesController.GetInstance(
                typeof(WishlistedDataController))
                as WishlistedDataController;

            var updatedDataController = singletonInstancesController.GetInstance(
                typeof(UpdatedDataController))
                as UpdatedDataController;

            var sessionRecordsController = singletonInstancesController.GetInstance(
                typeof(SessionRecordsController))
                as SessionRecordsController;

            // var convertProductRecordToIndexDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertProductCoreToIndexDelegate<ProductRecords>))
            //     as ConvertProductCoreToIndexDelegate<ProductRecords>;

            // var getRecordsDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetRecordsDirectoryDelegate))
            //     as GetRecordsDirectoryDelegate;

            // Controllers.Data

            var productsDataController = singletonInstancesController.GetInstance(
                typeof(ProductsDataController))
                as ProductsDataController;

            var accountProductsDataController = singletonInstancesController.GetInstance(
                typeof(AccountProductsDataController))
                as AccountProductsDataController;

            var gameDetailsDataController = singletonInstancesController.GetInstance(
                typeof(GameDetailsDataController))
                as GameDetailsDataController;

            var gameProductDataDataController = singletonInstancesController.GetInstance(
                typeof(GameProductDataDataController))
                as GameProductDataDataController;

            var apiProductsDataController = singletonInstancesController.GetInstance(
                typeof(ApiProductsDataController))
                as ApiProductsDataController;

            var productScreenshotsDataController = singletonInstancesController.GetInstance(
                typeof(ProductScreenshotsDataController))
                as ProductScreenshotsDataController;

            var productDownloadsDataController = singletonInstancesController.GetInstance(
                typeof(ProductDownloadsDataController))
                as ProductDownloadsDataController;

            var productRoutesDataController = singletonInstancesController.GetInstance(
                typeof(ProductRoutesDataController))
                as ProductRoutesDataController;

            var validationResultsDataController = singletonInstancesController.GetInstance(
                typeof(ValidationResultsDataController))
                as ValidationResultsDataController;

            #region Activity Controllers

            var authorizeActivity = singletonInstancesController.GetInstance(
                typeof(AuthorizeActivity))
                as AuthorizeActivity;

            var updateProductsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateProductsActivity))
                as UpdateProductsActivity;

            var updateAccountProductsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateAccountProductsActivity))
                as UpdateAccountProductsActivity;

            var updateUpdatedActivity = singletonInstancesController.GetInstance(
                typeof(UpdateUpdatedActivity))
                as UpdateUpdatedActivity;

            var updateWishlistedActivity = singletonInstancesController.GetInstance(
                typeof(UpdateWishlistedActivity))
                as UpdateWishlistedActivity;

            var updateGameProductDataByProductsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateGameProductDataByProductsActivity))
                as UpdateGameProductDataByProductsActivity;

            var updateApiProductsByProductsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateApiProductsByProductsActivity))
                as UpdateApiProductsByProductsActivity;

            // await updateApiProductsByProductsActivity.ProcessActivityAsync(applicationStatus);

            var updateGameDetailsByAccountProductsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateGameDetailsByAccountProductsActivity))
                as UpdateGameDetailsByAccountProductsActivity;

            // var convertTypeDependenciesToStringsDelegate = singletonInstancesController.GetInstance(
            //     typeof(ConvertTypeDependenciesToStringsDelegate))
            //     as ConvertTypeDependenciesToStringsDelegate;

            // foreach (var dependency in
            //     convertTypeDependenciesToStringsDelegate.Convert(typeof(UpdateGameDetailsByAccountProductsActivity)))
            //     System.Console.WriteLine(dependency);                

            #region Update.Screenshots

            // var updateScreenshotsAsyncDelegate = singletonInstancesController.GetInstance(
            //     typeof(UpdateScreenshotsAsyncDelegate))
            //     as UpdateScreenshotsAsyncDelegate;

            var updateScreenshotsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateScreenshotsActivity))
                as UpdateScreenshotsActivity;

            #endregion

            // dependencies for download controllers

            var getProductImageUriDelegate = singletonInstancesController.GetInstance(
                typeof(GetProductImageUriDelegate))
                as GetProductImageUriDelegate;

            var getAccountProductImageUriDelegate = singletonInstancesController.GetInstance(
                typeof(GetAccountProductImageUriDelegate))
                as GetAccountProductImageUriDelegate;

            var getProductsImagesDownloadSourcesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(GetProductImagesDownloadSourcesAsyncDelegate))
                as GetProductImagesDownloadSourcesAsyncDelegate;

            var getAccountProductsImagesDownloadSourcesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(GetAccountProductImagesDownloadSourcesAsyncDelegate))
                as GetAccountProductImagesDownloadSourcesAsyncDelegate;

            var getScreenshotsDownloadSourcesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(GetScreenshotsDownloadSourcesAsyncDelegate))
                as GetScreenshotsDownloadSourcesAsyncDelegate;

            var routingController = singletonInstancesController.GetInstance(
                typeof(RoutingController))
                as RoutingController;

            var itemizeGameDetailsManualUrlsAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeGameDetailsManualUrlsAsyncDelegate))
                as ItemizeGameDetailsManualUrlsAsyncDelegate;          

            var itemizeGameDetailsDirectoriesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeGameDetailsDirectoriesAsyncDelegate))
                as ItemizeGameDetailsDirectoriesAsyncDelegate;

            var itemizeGameDetailsFilesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeGameDetailsFilesAsyncDelegate))
                as ItemizeGameDetailsFilesAsyncDelegate;

            // product files are driven through gameDetails manual urls
            // so this sources enumerates all manual urls for all updated game details
            var getManualUrlDownloadSourcesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(GetManualUrlDownloadSourcesAsyncDelegate))
                as GetManualUrlDownloadSourcesAsyncDelegate;

            // schedule download controllers             

            var updateProductsImagesDownloadsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateProductImagesDownloadsActivity))
                as UpdateProductImagesDownloadsActivity;           

            var updateAccountProductsImagesDownloadsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateAccountProductImagesDownloadsActivity))
                as UpdateAccountProductImagesDownloadsActivity;            

            var updateScreenshotsDownloadsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateProductScreenshotsDownloadsActivity))
                as UpdateProductScreenshotsDownloadsActivity;

            var updateProductFilesDownloadsActivity = singletonInstancesController.GetInstance(
                typeof(UpdateProductFilesDownloadsActivity))
                as UpdateProductFilesDownloadsActivity;               

            // downloads processing

            var formatUriRemoveSessionDelegate = singletonInstancesController.GetInstance(
                typeof(FormatUriRemoveSessionDelegate))
                as FormatUriRemoveSessionDelegate;

            var confirmValidationExpectedDelegate = singletonInstancesController.GetInstance(
                typeof(ConfirmValidationExpectedDelegate))
                as ConfirmValidationExpectedDelegate;

            var formatValidationUriDelegate = singletonInstancesController.GetInstance(
                typeof(FormatValidationUriDelegate))
                as FormatValidationUriDelegate;

            var formatValidationFileDelegate = singletonInstancesController.GetInstance(
                typeof(FormatValidationFileDelegate))
                as FormatValidationFileDelegate;

            var downloadValidationFileAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(DownloadValidationFileAsyncDelegate))
                as DownloadValidationFileAsyncDelegate;

            var downloadManualUrlFileAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(DownloadManualUrlFileAsyncDelegate))
                as DownloadManualUrlFileAsyncDelegate;          

            var downloadProductImageAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(DownloadProductImageAsyncDelegate))
                as DownloadProductImageAsyncDelegate;

            var productsImagesDownloadActivity = singletonInstancesController.GetInstance(
                typeof(DownloadProductImagesActivity))
                as DownloadProductImagesActivity;              

            var accountProductsImagesDownloadActivity = singletonInstancesController.GetInstance(
                typeof(DownloadAccountProductImagesActivity))
                as DownloadAccountProductImagesActivity;

            var screenshotsDownloadActivity = singletonInstancesController.GetInstance(
                typeof(DownloadProductScreenshotsActivity))
                as DownloadProductScreenshotsActivity;               

            var productFilesDownloadActivity = singletonInstancesController.GetInstance(
                typeof(DownloadProductFilesActivity))
                as DownloadProductFilesActivity;            

            // validation controllers

            var validationResultController = singletonInstancesController.GetInstance(
                typeof(ValidationResultController))
                as ValidationResultController;

            var convertFileToMd5HashDelegate = singletonInstancesController.GetInstance(
                typeof(ConvertFileToMd5HashDelegate))
                as ConvertFileToMd5HashDelegate;             

            var productFileValidationController = singletonInstancesController.GetInstance(
                typeof(FileValidationController))
                as FileValidationController;             

            var validateProductFilesActivity = singletonInstancesController.GetInstance(
                typeof(ValidateProductFilesActivity))
                as ValidateProductFilesActivity;               

            var itemizeAllGameDetailsDirectoriesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeAllGameDetailsDirectoriesAsyncDelegate))
                as ItemizeAllGameDetailsDirectoriesAsyncDelegate;

            var itemizeAllProductFilesDirectoriesAsyncDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeAllProductFilesDirectoriesAsyncDelegate))
                as ItemizeAllProductFilesDirectoriesAsyncDelegate;

            var itemizeDirectoryFilesDelegate = singletonInstancesController.GetInstance(
                typeof(ItemizeDirectoryFilesDelegate))
                as ItemizeDirectoryFilesDelegate;

            // PrintDependencies(
            //     itemizeAllGameDetailsDirectoriesAsyncDelegate, // expected items (directories for gameDetails)
            //     itemizeAllProductFilesDirectoriesAsyncDelegate, // actual items (directories in productFiles)
            //     itemizeDirectoryFilesDelegate, // detailed items (files in directory)
            //     formatValidationFileDelegate, // supplementary items (validation files)
            //     recycleDelegate,
            //     directoryController,
            //     statusController);

            var directoryCleanupActivity = new CleanupActivity(
                Entity.Directories,
                itemizeAllGameDetailsDirectoriesAsyncDelegate, // expected items (directories for gameDetails)
                itemizeAllProductFilesDirectoriesAsyncDelegate, // actual items (directories in productFiles)
                itemizeDirectoryFilesDelegate, // detailed items (files in directory)
                formatValidationFileDelegate, // supplementary items (validation files)
                recycleDelegate,
                directoryController,
                statusController);

            var itemizeAllUpdatedGameDetailsManualUrlFilesAsyncDelegate =
                new ItemizeAllUpdatedGameDetailsManualUrlFilesAsyncDelegate(
                    updatedDataController,
                    gameDetailsDataController,
                    itemizeGameDetailsFilesAsyncDelegate,
                    statusController);

            var itemizeAllUpdatedProductFilesAsyncDelegate =
                new ItemizeAllUpdatedProductFilesAsyncDelegate(
                    updatedDataController,
                    gameDetailsDataController,
                    itemizeGameDetailsDirectoriesAsyncDelegate,
                    directoryController,
                    statusController);

            var itemizePassthroughDelegate = new ItemizePassthroughDelegate();

            var fileCleanupActivity = new CleanupActivity(
                Entity.Files,
                itemizeAllUpdatedGameDetailsManualUrlFilesAsyncDelegate, // expected items (files for updated gameDetails)
                itemizeAllUpdatedProductFilesAsyncDelegate, // actual items (updated product files)
                itemizePassthroughDelegate, // detailed items (passthrough)
                formatValidationFileDelegate, // supplementary items (validation files)
                recycleDelegate,
                directoryController,
                statusController);

            var cleanupUpdatedActivity = new CleanupUpdatedActivity(
                updatedDataController,
                statusController);

            #endregion

            #region ArgsDefinition and args

            var argsDefinitions = await argsDefinitionStashController.GetDataAsync(applicationStatus);

            if (args == null ||
                args.Length == 0)
                args = argsDefinitions.DefaultArgs.Split(" ");

            var convertArgsToRequestsDelegate = singletonInstancesController.GetInstance(
                typeof(ConvertArgsToRequestsDelegate))
                as ConvertArgsToRequestsDelegate;

            await foreach (var request in convertArgsToRequestsDelegate.ConvertAsync(args, applicationStatus))
            {
                System.Console.WriteLine($"{request.Method.ToUpper()} {request.Collection}");
            }

            #endregion

            #region Core Activities Loop

            // foreach (var activityContext in activityContextQueue)
            // {
            //     if (!activityContextToActivityControllerMap.ContainsKey(activityContext))
            //     {
            //         await statusController.WarnAsync(
            //             applicationStatus,
            //             activityContextController.ToString(activityContext) + " is not mapped to an Activity.");
            //         continue;
            //     }

            //     var activity = activityContextToActivityControllerMap[activityContext];
            //     try
            //     {
            //         await activity.ProcessActivityAsync(applicationStatus);
            //     }
            //     catch (AggregateException ex)
            //     {
            //         var itemizeInnerExceptionsDelegate = new ItemizeInnerExceptionsDelegate();
            //         var convertTreeToEnumerableDelegate = new ConvertTreeToEnumerableDelegate<Exception>(itemizeInnerExceptionsDelegate);

            //         var errorMessages = new List<string>();
            //         foreach (var innerException in convertTreeToEnumerableDelegate.Convert(ex))
            //             errorMessages.Add(innerException.Message);

            //         var combinedErrorMessages = string.Join(Models.Separators.Separators.Common.Comma, errorMessages);

            //         await statusController.FailAsync(applicationStatus, combinedErrorMessages);

            //         var failureDumpUri = "failureDump.json";
            //         await jsonSerializedStorageController.SerializePushAsync(failureDumpUri, applicationStatus, applicationStatus);

            //         await consoleInputOutputController.OutputOnRefreshAsync(
            //             "GoodOfflineGames.exe has encountered fatal error(s): " +
            //             combinedErrorMessages +
            //             $".\nPlease refer to {failureDumpUri} for further details.\n" +
            //             "Press ENTER to close the window...");

            //         consoleController.ReadLine();

            //         return;
            //     }
            // }

            #endregion

            // TODO: Present session results

            if (applicationStatus.SummaryResults != null)
            {
                foreach (var line in applicationStatus.SummaryResults)
                    await consoleInputOutputController.OutputOnRefreshAsync(string.Join(" ", applicationStatus.SummaryResults));
            }
            else
            {
                await consoleInputOutputController.OutputContinuousAsync(applicationStatus, string.Empty, "All tasks are complete. Press ENTER to exit...");
            }

            System.Console.ReadLine();
        }
    }
}