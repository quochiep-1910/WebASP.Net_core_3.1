using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Utilities.Slides;
using System.Collections.Generic;

namespace eShop.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { set; get; }
        public List<ProductViewModel> FeaturedProducts { set; get; }
        public List<ProductViewModel> LastestProducts { set; get; }
    }
}