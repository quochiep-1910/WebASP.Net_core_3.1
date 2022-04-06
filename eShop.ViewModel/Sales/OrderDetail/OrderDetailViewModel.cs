using Microsoft.AspNetCore.Mvc;
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

      
        [Display(Name = "Giá")]
        public decimal Price { set; get; }
    }
}