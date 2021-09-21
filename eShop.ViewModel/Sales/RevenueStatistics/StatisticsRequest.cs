using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModels.Sales.RevenueStatistics
{
    public class StatisticsRequest
    {
        [Display(Name = "Ngày bắt đầu")]
        public string FromDate { set; get; }

        [Display(Name = "Ngày kết thúc")]
        public string ToDate { set; get; }
    }
}