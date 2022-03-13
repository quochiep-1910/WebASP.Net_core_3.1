using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.RevenueStatistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<bool> CreateOrder(OrderCreateRequest request);

        Task<bool> UpdateOrder(OrderUpdateRequest orderUpdate);

        Task<List<RevenueStatisticViewModel>> RevenueStatistic(StatisticsRequest request);

        Task<OrderViewModel> GetById(int id);

        Task<bool> DeleteOrder(int id);

        Task<PagedResult<OrderViewModel>> GetPagings(GetOrderPagingRequest request);
    }
}