﻿using Interfaces.Reporting;
using Interfaces.Storage;
using Interfaces.ProductTypes;
using Interfaces.Collection;
using Interfaces.Network;
using Interfaces.Serialization;
using Interfaces.Politeness;
using Interfaces.UpdateDependencies;

using GOG.Models;

using GOG.TaskActivities.Abstract;

namespace GOG.TaskActivities.Update.Products
{
    public class GameDetailsUpdateController : ProductUpdateController<GameDetails, AccountProduct>
    {
        public GameDetailsUpdateController(
            IProductTypeStorageController productStorageController,
            ICollectionController collectionController,
            INetworkController networkController,
            ISerializationController<string> serializationController,
            IPolitenessController politenessController,
            IUpdateUriController updateUriController,
            IRequiredUpdatesController requiredUpdatesController,
            IConnectionController connectionController,
            ITaskReportingController taskReportingController) :
            base(productStorageController,
                collectionController,
                networkController,
                serializationController,
                politenessController,
                updateUriController,
                requiredUpdatesController,
                null, // skipUpdateController
                null, // dataDecodingController
                connectionController,
                taskReportingController)
        {
            updateProductType = ProductTypes.GameDetails;
            listProductType = ProductTypes.AccountProduct;

            displayProductName = "game details";
        }
    }
}
