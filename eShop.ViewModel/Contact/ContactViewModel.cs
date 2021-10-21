using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.ViewModels.Contact
{
    public class ContactViewModel
    {
        public int Id { set; get; }

        [Display(Name = "Họ tên")]
        public string Name { set; get; }

        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }

        [Display(Name = "Tin nhắn")]
        public string Message { set; get; }

        [Display(Name = "Trạng thái")]
        public Status Status { set; get; }
    }
}