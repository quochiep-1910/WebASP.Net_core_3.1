using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Sales
{
    public interface IOrderService
    {
        Task<int> Create(OrderCreateRequest request);

        Task<int> Delete(int orderId);

        Task<OrderViewModel> GetById(int orderId);

        Task<List<OrderViewModel>> GetAll();
    }
}