using eShop.ViewModels.Sales.Order;
using System.Collections.Generic;

namespace eShop.WebApp.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }

        public OrderCreateRequest CheckoutModel { get; set; }
    }
}