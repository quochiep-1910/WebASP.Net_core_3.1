using eShop.ViewModels.Common;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class GetManageProductCategoryPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        //public List<int> CategoryIds { get; set; }
        public string LanguageId { get; set; }
    }
}