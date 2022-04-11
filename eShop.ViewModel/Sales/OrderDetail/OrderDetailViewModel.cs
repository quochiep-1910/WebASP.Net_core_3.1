using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.Sales.OrderDetail
{
    public class OrderDetailViewModel
    {
        public int OrderId { set; get; }

        [Display(Name = "Mã sản phẩm")]
        public int ProductId { set; get; }

        [Display(Name = "Số lượng")]
        public int Quantity { set; get; }

        [Display(Name = "Giá Tổng")]
        public decimal Price { set; get; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Miêu tả sản phẩm")]
        public string Description { set; get; }

        public string ThumbnailImage { get; set; }
    }
}