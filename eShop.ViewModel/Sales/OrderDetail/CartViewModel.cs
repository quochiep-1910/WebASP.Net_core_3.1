using eShop.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Sales.OrderDetail
{
    public class CartViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}