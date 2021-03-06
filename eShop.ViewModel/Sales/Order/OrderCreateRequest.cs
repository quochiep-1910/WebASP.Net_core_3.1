using eShop.ViewModels.Sales.OrderDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.ViewModels.Sales.Order
{
    public class OrderCreateRequest
    {
        public Guid UserId { set; get; }
        public int ProductId { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime OrderDate { set; get; }

        [Display(Name = "Tên")]
        public string ShipName { set; get; }

        [Display(Name = "Địa chỉ nhận")]
        public string ShipAddress { set; get; }

        [Display(Name = "Email")]
        public string ShipEmail { set; get; }

        [Display(Name = "Số điện thoại")]
        public string ShipPhoneNumber { set; get; }

        [Display(Name = "Số Lượng")]
        public int Quantity { set; get; }

        [Display(Name = "Giá")]
        public decimal Price { set; get; }

        [Display(Name = "Trạng thái")]
        public OrderStatus Status { set; get; }

        public List<OrderDetailViewModel> OrderDetails { set; get; }
    }
}