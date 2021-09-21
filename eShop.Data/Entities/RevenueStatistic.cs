using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Data.Entities
{
    public class RevenueStatistic
    {
        public DateTime Date { get; set; }
        public decimal? Revenues { get; set; }
        public decimal? Benefit { get; set; }
    }
}