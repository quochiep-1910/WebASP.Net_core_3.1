using eShop.Data.Entities;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.RevenueStatistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Sales
{
    public interface IOrderService
    {
        Task<int> Create(OrderCreateRequest request);

        Task<int> Update(OrderUpdateRequest request);

        Task<int> Delete(int orderId);

        Task<OrderViewModel> GetById(int orderId);

        Task<List<OrderViewModel>> GetAll();

        Task<PagedResult<OrderViewModel>> GetOrderPaging(GetOrderPagingRequest request);

        Task<IEnumerable<RevenueStatistic>> GetRevenueStatistic(StatisticsRequest request);
    }
}