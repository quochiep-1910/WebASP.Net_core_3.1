using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class GetManageProductCategoryPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        //public List<int> CategoryIds { get; set; }
        public string LanguageId { get; set; }
    }
}