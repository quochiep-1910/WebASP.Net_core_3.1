using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { set; get; }
        public List<ProductViewModel> FeaturedProducts { set; get; }
    }
}