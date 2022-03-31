using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.System.Users
{
    public class UserDeleteRequest
    {
        public string Id { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }
    }
}