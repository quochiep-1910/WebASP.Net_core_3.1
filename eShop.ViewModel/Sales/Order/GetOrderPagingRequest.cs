using eShop.ViewModels.Common;

namespace eShop.ViewModels.Sales.Order
{
    public class GetOrderPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}