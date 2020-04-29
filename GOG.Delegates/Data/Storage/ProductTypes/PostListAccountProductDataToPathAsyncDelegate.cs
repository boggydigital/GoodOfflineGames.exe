using System.Collections.Generic;
using Attributes;
using Delegates.Data.Storage;
using Interfaces.Delegates.Data;
using Interfaces.Delegates.GetPath;
using GOG.Models;

namespace GOG.Delegates.Data.Storage.ProductTypes
{
    public class PostListAccountProductDataToPathAsyncDelegate : PostJSONDataToPathAsyncDelegate<List<AccountProduct>>
    {
        [Dependencies(
            "GOG.Delegates.Data.Storage.ProductTypes.PostListAccountProductDataAsyncDelegate,GOG.Delegates",
            "Delegates.GetPath.ProductTypes.GetAccountProductsPathDelegate,Delegates")]
        public PostListAccountProductDataToPathAsyncDelegate(
            IPostDataAsyncDelegate<List<AccountProduct>> postListAccountProductDataAsyncDelegate,
            IGetPathDelegate getAccountProductPathDelegate) :
            base(
                postListAccountProductDataAsyncDelegate,
                getAccountProductPathDelegate)
        {
            // ...
        }
    }
}