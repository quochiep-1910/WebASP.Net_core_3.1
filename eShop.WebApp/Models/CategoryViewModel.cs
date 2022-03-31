using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;

namespace eShop.WebApp.Models
{
    public class CategoryViewModel
    {
        public ProductCategoryViewModel Category { get; set; }
        public PagedResult<ProductViewModel> Products { get; set; }
    }
}