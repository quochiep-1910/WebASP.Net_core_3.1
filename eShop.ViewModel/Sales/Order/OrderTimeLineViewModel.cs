using eShop.ViewModels.Sales.OrderDetail;
using System.Collections.Generic;

namespace eShop.ViewModels.Sales.Order
{
    public class OrderTimeLineViewModel
    {
        public List<OrderDetailTimeLineViewModel> OrderDetails { set; get; }
    }
}