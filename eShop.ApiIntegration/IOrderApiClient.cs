using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.ViewModels.Sales.RevenueStatistics;
using eShop.ViewModels.Utilities.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<int> CreateOrder(OrderCreateRequest request);

        Task<bool> CreateOrderDetail(List<OrderDetailViewModel> request);

        Task<bool> UpdateOrder(OrderUpdateRequest orderUpdate);

        Task<List<RevenueStatisticViewModel>> RevenueStatistic(StatisticsRequest request);

        Task<OrderViewModel> GetById(int id);

        Task<bool> DeleteOrder(int id);

        Task<PagedResult<OrderViewModel>> GetPagings(GetOrderPagingRequest request);

        /// <summary>
        /// Get total order now
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalOrder();

        /// <summary>
        /// send email with sendgrid
        /// </summary>
        /// <param name="sendMailViewModel"></param>
        /// <returns></returns>
        Task<bool> CreateSendEmail(SendMailViewModel sendMailViewModel);

        /// <summary>
        /// Change status order when momo payment success
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ChangeStatusOrder(ChangeStatusOrder request);
    }
}