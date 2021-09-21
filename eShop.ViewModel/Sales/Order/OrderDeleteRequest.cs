using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModels.Sales.Order
{
    public class OrderDeleteRequest
    {
        public int Id { set; get; }

        [Display(Name = "Tên")]
        public string ShipName { set; get; }

        [Display(Name = "Địa chỉ nhận")]
        public string ShipAddress { set; get; }

        [Display(Name = "Email")]
        public string ShipEmail { set; get; }

        [Display(Name = "Số điện thoại")]
        public string ShipPhoneNumber { set; get; }
    }
}