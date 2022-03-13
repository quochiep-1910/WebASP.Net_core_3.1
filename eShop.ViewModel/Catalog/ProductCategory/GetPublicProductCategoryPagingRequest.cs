using eShop.ViewModels.Common;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class GetPublicProductCategoryPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}