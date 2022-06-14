using System;
using System.ComponentModel.DataAnnotations;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.ViewModels.Sales.OrderDetail
{
    public class OrderDetailTimeLineViewModel
    {
        public string UserId { set; get; }
        public int OrderId { set; get; }

        [Display(Name = "Mã sản phẩm")]
        public int ProductId { set; get; }

        [Display(Name = "Tên")]
        public string ShipName { set; get; }

        [Display(Name = "Địa chỉ nhận")]
        public string ShipAddress { set; get; }

        [Display(Name = "Email")]
        public string ShipEmail { set; get; }

        [Display(Name = "Số điện thoại")]
        public string ShipPhoneNumber { set; get; }

        [Display(Name = "Số lượng")]
        public int Quantity { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime OrderDate { set; get; }

        [Display(Name = "Giá Tổng")]
        public decimal Price { set; get; }

        [Display(Name = "Trạng thái")]
        public OrderStatus Status { set; get; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Miêu tả sản phẩm")]
        public string Description { set; get; }

        public string ThumbnailImage { get; set; }
    }
}