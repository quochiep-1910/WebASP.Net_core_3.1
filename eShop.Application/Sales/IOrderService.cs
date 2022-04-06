using eShop.Data.Entities;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
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

        /// <summary>
        /// Add order detail when order add
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<int> CreateOrderDetail(List<OrderDetailViewModel> request);

        /// <summary>
        /// Get total Order now
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalOrder();
    }
}