using System.Collections.Generic;

using Interfaces.Delegates.GetPath;

using Interfaces.Controllers.SerializedStorage;

using Interfaces.Status;

using Attributes;

using Models.Records;
using Models.Dependencies;

namespace Controllers.Stash.Records
{
    public class UpdatedRecordsStashController : StashController<List<ProductRecords>>
    {
        [Dependencies(
            "Delegates.GetPath.Records.GetUpdatedRecordsPathDelegate,Delegates",
            Dependencies.DefaultSerializedStorageController,
            "Controllers.Status.StatusController,Controllers")]
        public UpdatedRecordsStashController(
            IGetPathDelegate getUpdatedRecordsPathDelegate,
            ISerializedStorageController serializedStorageController,
            IStatusController statusController) :
            base(
                getUpdatedRecordsPathDelegate,
                serializedStorageController,
                statusController)
        {
            // ...
        }
    }
}