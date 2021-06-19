using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class GetPublicProductCategoryPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}