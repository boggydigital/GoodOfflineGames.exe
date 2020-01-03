﻿using Interfaces.Delegates.Recycle;
using Interfaces.Delegates.Itemize;
using Interfaces.Delegates.Format;

using Interfaces.Controllers.Directory;
using Interfaces.Controllers.Logs;

using Attributes;

using Models.ProductTypes;

namespace GOG.Delegates.Respond.Cleanup.ProductTypes
{
    [RespondsToRequests(Method = "cleanup", Collection = "files")]
    public class RespondToCleanupFilesRequestDelegate : RespondToCleanupRequestDelegate<ProductFile>
    {
        [Dependencies(
            "GOG.Delegates.Itemize.ItemizeAllGameDetailsDirectoriesAsyncDelegate,GOG.Delegates",
            "GOG.Delegates.Itemize.ItemizeAllProductFilesDirectoriesAsyncDelegate,GOG.Delegates",
            "Delegates.Itemize.ItemizeDirectoryFilesDelegate,Delegates",
            "Delegates.Format.Uri.FormatValidationFileDelegate,Delegates",
            "Delegates.Recycle.RecycleDelegate,Delegates",
            "Controllers.Directory.DirectoryController,Controllers",
            "Controllers.Logs.ActionLogController,Controllers")]
        public RespondToCleanupFilesRequestDelegate(
            IItemizeAllAsyncDelegate<string> itemizeAllExpectedProductFilesAsyncDelegate,
            IItemizeAllAsyncDelegate<string> itemizeAllActualProductFilesAsyncDelegate,
            IItemizeDelegate<string, string> itemizeDetailsDelegate,
            IFormatDelegate<string, string> formatSupplementaryItemDelegate,
            IRecycleDelegate recycleDelegate,
            IDirectoryController directoryController,
            IActionLogController actionLogController) :
            base(
                itemizeAllExpectedProductFilesAsyncDelegate,
                itemizeAllActualProductFilesAsyncDelegate,
                itemizeDetailsDelegate,
                formatSupplementaryItemDelegate,
                recycleDelegate,
                directoryController,
                actionLogController)
        {
            // ...
        }
    }
}