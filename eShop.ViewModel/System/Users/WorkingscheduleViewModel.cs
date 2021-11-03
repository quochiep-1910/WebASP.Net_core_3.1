using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class WorkingscheduleViewModel
    {
        public int Id { set; get; }

        [Display(Name = "Từ Ngày")]
        public DateTime StartDate { set; get; }

        [Display(Name = "Đến Ngày")]
        public DateTime EndDate { set; get; }

        [Display(Name = "Lý Do")]
        public string LyDo { set; get; }

        [Display(Name = "Đăng kí cho")]
        public string UserName { set; get; }

        [Display(Name = "Diễn giải")]
        public string Message { set; get; }
    }
}