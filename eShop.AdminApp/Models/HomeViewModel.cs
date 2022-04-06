using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.RevenueStatistics;
using System.Collections.Generic;

namespace eShop.AdminApp.Models
{
    public class HomeViewModel
    {
        public int TotalOrder { get; set; }
        public int TotalUser { get; set; }
        public int TotalProducts { get; set; }
        public List<RevenueStatisticViewModel> RevenueStatisticViews { get; set; }

        /// <summary>
        /// Danh sách sản phẩm bán chạy
        /// </summary>
        public OptionProductView ListProductTopSelling { get; set; }

        public class OptionProductView
        {
            public List<ProductViewModel> Items { set; get; }
            public PagedResultBase Paging { set; get; }
        }
    }
}