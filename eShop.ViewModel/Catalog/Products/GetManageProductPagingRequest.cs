using eShop.ViewModels.Common;

namespace eShop.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        //public List<int> CategoryIds { get; set; }
        public string LanguageId { get; set; }

        public int? CategoryId { get; set; }
    }
}