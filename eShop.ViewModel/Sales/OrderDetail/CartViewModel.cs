using eShop.ViewModels.Catalog.Products;

namespace eShop.ViewModels.Sales.OrderDetail
{
    public class CartViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}