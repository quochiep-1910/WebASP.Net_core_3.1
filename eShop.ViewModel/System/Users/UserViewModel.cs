﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.System.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Ngày Sinh")]
        public DateTime Dob { set; get; }

        [Display(Name = "Trạng Thái")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Trạng Thái Email")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Ngày Mở Khoá Tài khoản")]
        public DateTimeOffset? LockoutEnd { set; get; }

        public IList<string> Roles { get; set; }
    }
}