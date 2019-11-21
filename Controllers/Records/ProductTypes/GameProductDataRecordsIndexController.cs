using Interfaces.Controllers.Data;
using Interfaces.Status;

using Models.Records;

namespace Controllers.Records.ProductTypes
{
    public class GameProductDataRecordsIndexController : IndexRecordsController
    {
        public GameProductDataRecordsIndexController(
            IDataController<ProductRecords> gameProductDataRecordsController,
            IStatusController statusController) :
            base(
                gameProductDataRecordsController,
                statusController)
        {
            // ...
        }
    }
}