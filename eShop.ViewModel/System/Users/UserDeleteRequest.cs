using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class UserDeleteRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }
    }
}