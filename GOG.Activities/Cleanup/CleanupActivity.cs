﻿using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Interfaces.Delegates.MoveToRecycleBin;

using Interfaces.Controllers.Directory;

using Interfaces.Status;
using Interfaces.Enumeration;
using Interfaces.ContextDefinitions;

namespace GOG.Activities.Cleanup
{
    public class CleanupActivity : Activity
    {
        private Context context;
        private IEnumerateAllAsyncDelegate<string> expectedItemsEnumarateDelegate;
        private IEnumerateAllAsyncDelegate<string> actualItemsEnumerateDelegate;
        private IEnumerateDelegate<string> itemsDetailsEnumerateDelegate;
        private IEnumerateDelegate<string> supplementaryItemsEnumerateDelegate;
        private IMoveToRecycleBinDelegate moveToRecycleBinDelegate;
        private IDirectoryController directoryController;

        public CleanupActivity(
            Context context,
            IEnumerateAllAsyncDelegate<string> expectedItemsEnumarateDelegate,
            IEnumerateAllAsyncDelegate<string> actualItemsEnumerateDelegate,
            IEnumerateDelegate<string> itemsDetailsEnumerateDelegate,
            IEnumerateDelegate<string> supplementaryItemsEnumerateDelegate,
            IMoveToRecycleBinDelegate moveToRecycleBinDelegate,
            IDirectoryController directoryController,
            IStatusController statusController) :
            base(statusController)
        {
            this.context = context;
            this.expectedItemsEnumarateDelegate = expectedItemsEnumarateDelegate;
            this.actualItemsEnumerateDelegate = actualItemsEnumerateDelegate;
            this.itemsDetailsEnumerateDelegate = itemsDetailsEnumerateDelegate;
            this.supplementaryItemsEnumerateDelegate = supplementaryItemsEnumerateDelegate;
            this.moveToRecycleBinDelegate = moveToRecycleBinDelegate;
            this.directoryController = directoryController;
        }

        public override async Task ProcessActivityAsync(IStatus status)
        {
            var cleanupTask = await statusController.CreateAsync(status, $"Cleanup {context}");

            var expectedItems = await expectedItemsEnumarateDelegate.EnumerateAllAsync(status);
            var actualItems = await actualItemsEnumerateDelegate.EnumerateAllAsync(status);

            var unexpectedItems = actualItems.Except(expectedItems);
            var cleanupItems = new List<string>();

            foreach (var unexpectedItem in unexpectedItems)
                foreach (var detailedItem in itemsDetailsEnumerateDelegate.Enumerate(unexpectedItem))
                {
                    cleanupItems.Add(detailedItem);
                    cleanupItems.AddRange(supplementaryItemsEnumerateDelegate.Enumerate(detailedItem));
                }

            var moveToRecycleBinTask = await statusController.CreateAsync(status, "Move unexpected items to recycle bin");
            var current = 0;

            foreach (var item in cleanupItems)
            {
                await statusController.UpdateProgressAsync(
                    moveToRecycleBinTask,
                    ++current,
                    cleanupItems.Count,
                    item);

                moveToRecycleBinDelegate.MoveToRecycleBin(item);
            }

            // check if any of the directories are left empty and delete
            var emptyDirectories = new List<string>();
            foreach (var item in cleanupItems)
            {
                var directory = Path.GetDirectoryName(item);
                if (!emptyDirectories.Contains(directory) &&
                    directoryController.EnumerateFiles(directory).Count() == 0 &&
                    directoryController.EnumerateDirectories(directory).Count() == 0)
                    emptyDirectories.Add(directory);
            }

            foreach (var directory in emptyDirectories)
                directoryController.Delete(directory);

            await statusController.CompleteAsync(moveToRecycleBinTask);
        }
    }
}
