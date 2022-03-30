using System;
using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.System.Users
{
    public class RegisterRequest
    {
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }

        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { set; get; }

        public string origin { get; set; }
    }
}