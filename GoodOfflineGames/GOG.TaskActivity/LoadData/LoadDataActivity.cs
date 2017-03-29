﻿using System.Threading.Tasks;

using Interfaces.Data;
using Interfaces.Status;

namespace GOG.Activities.Load
{
    public class LoadDataActivity: Activity
    {
        private ILoadDelegate[] loadDelegates;

        public LoadDataActivity(
            IStatusController statusController,
            params ILoadDelegate[] loadDelegates): 
            base(statusController)
        {
            this.loadDelegates = loadDelegates;
        }

        public override async Task ProcessActivityAsync(IStatus status)
        {
            var loadDataTask = statusController.Create(status, "Load existing data");
            for (var ii = 0; ii < loadDelegates.Length; ii++)
            {
                statusController.UpdateProgress(
                    loadDataTask, ii + 1,
                    loadDelegates.Length,
                    "Existing data");

                await loadDelegates[ii].LoadAsync();
            }
            statusController.Complete(loadDataTask);
        }
    }
}
