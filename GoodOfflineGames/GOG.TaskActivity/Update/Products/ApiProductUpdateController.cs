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
    public class ApiProductUpdateController : ProductUpdateController<ApiProduct, Product>
    {
        public ApiProductUpdateController(
            IProductTypeStorageController productStorageController,
            ICollectionController collectionController,
            INetworkController networkController,
            ISerializationController<string> serializationController,
            IPolitenessController politenessController,
            IUpdateUriController updateUriController,
            ITaskReportingController taskReportingController) :
            base(productStorageController,
                collectionController,
                networkController,
                serializationController,
                politenessController,
                updateUriController,
                null, // requiredUpdatesController
                null, // skipUpdateController
                null, // dataDecodingController
                null, // connectionController
                taskReportingController)
        {
            updateProductType = ProductTypes.ApiProduct;
            listProductType = ProductTypes.Product;

            displayProductName = "API product";
        }
    }
}
