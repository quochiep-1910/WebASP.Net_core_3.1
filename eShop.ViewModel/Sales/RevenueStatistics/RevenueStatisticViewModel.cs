using System;

namespace eShop.ViewModels.Sales.RevenueStatistics
{
    public class RevenueStatisticViewModel
    {
        public DateTime Date { get; set; }
        public decimal? Revenues { get; set; }
        public decimal? Benefit { get; set; }
    }
}