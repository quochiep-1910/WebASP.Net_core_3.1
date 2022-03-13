using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.Sales.OrderDetail
{
    public class OrderDetailViewModel
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { set; get; }

        [Display(Name = "Số lượng")]
        public int Quantity { set; get; }

        [Display(Name = "Giá")]
        public decimal Price { set; get; }
    }
}