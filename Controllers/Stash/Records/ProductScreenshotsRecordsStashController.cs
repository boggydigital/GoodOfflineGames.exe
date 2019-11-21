using System.Collections.Generic;

using Interfaces.Delegates.GetPath;

using Interfaces.Controllers.SerializedStorage;

using Interfaces.Status;

using Models.Records;

namespace Controllers.Stash.Records
{
    public class ProductScreenshotsRecordsStashController : StashController<Dictionary<long, ProductRecords>>
    {
        public ProductScreenshotsRecordsStashController(
            IGetPathDelegate getProductScreenshotsRecordsPathDelegate,
            ISerializedStorageController serializedStorageController,
            IStatusController statusController) :
            base(
                getProductScreenshotsRecordsPathDelegate,
                serializedStorageController,
                statusController)
        {
            // ...
        }
    }
}