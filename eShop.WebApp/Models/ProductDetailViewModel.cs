using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public ProductCategoryViewModel Category { get; set; }
        public ProductViewModel Product { get; set; }
        public List<ProductViewModel> RelatedProducts { set; get; }
        public List<ProductImageViewModel> ProductImages { set; get; }
    }
}