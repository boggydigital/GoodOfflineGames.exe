﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Interfaces.Controllers.Data;
using Interfaces.Controllers.Directory;

using Interfaces.Delegates.Itemize;

using Interfaces.Status;

using Attributes;

using GOG.Models;

namespace GOG.Delegates.Itemize
{
    public class ItemizeAllUpdatedProductFilesAsyncDelegate : IItemizeAllAsyncDelegate<string>
    {
        readonly IDataController<long> updatedDataController;
        readonly IDataController<GameDetails> gameDetailsDataController;
        readonly IItemizeAsyncDelegate<GameDetails, string> itemizeGameDetailsDirectoriesAsyncDelegate;
        readonly IDirectoryController directoryController;
        readonly IStatusController statusController;

		[Dependencies(
			"Controllers.Data.ProductTypes.UpdatedDataController,Controllers",
			"GOG.Controllers.Data.ProductTypes.GameDetailsDataController,GOG.Controllers",
			"GOG.Delegates.Itemize.ItemizeGameDetailsDirectoriesAsyncDelegate,GOG.Delegates",
			"Controllers.Directory.DirectoryController,Controllers",
			"Controllers.Status.StatusController,Controllers")]
        public ItemizeAllUpdatedProductFilesAsyncDelegate(
            IDataController<long> updatedDataController,
            IDataController<GameDetails> gameDetailsDataController,
            IItemizeAsyncDelegate<GameDetails, string> itemizeGameDetailsDirectoriesAsyncDelegate,
            IDirectoryController directoryController,
            IStatusController statusController)
        {
            this.updatedDataController = updatedDataController;
            this.gameDetailsDataController = gameDetailsDataController;
            this.itemizeGameDetailsDirectoriesAsyncDelegate = itemizeGameDetailsDirectoriesAsyncDelegate;
            this.directoryController = directoryController;
            this.statusController = statusController;
        }

        public async IAsyncEnumerable<string> ItemizeAllAsync(IStatus status)
        {
            var enumerateUpdatedProductFilesTask = await statusController.CreateAsync(status, "Enumerate updated productFiles");

            var updatedIdsCount = await updatedDataController.CountAsync(enumerateUpdatedProductFilesTask);
            var current = 0;

            await foreach (var id in updatedDataController.ItemizeAllAsync(enumerateUpdatedProductFilesTask))
            {
                var gameDetails = await gameDetailsDataController.GetByIdAsync(id, enumerateUpdatedProductFilesTask);

                await statusController.UpdateProgressAsync(
                    enumerateUpdatedProductFilesTask,
                    ++current,
                    updatedIdsCount,
                    gameDetails.Title);

                var gameDetailsDirectories =
                    await itemizeGameDetailsDirectoriesAsyncDelegate.ItemizeAsync(
                        gameDetails,
                        enumerateUpdatedProductFilesTask);

                foreach (var gameDetailDirectory in gameDetailsDirectories)
                    foreach (var updatedFile in directoryController.EnumerateFiles(gameDetailDirectory))
                        yield return updatedFile;
            }

            await statusController.CompleteAsync(enumerateUpdatedProductFilesTask);
        }
    }
}
