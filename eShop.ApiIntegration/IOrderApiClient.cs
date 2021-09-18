using eShop.ViewModels.Sales.Order;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<bool> CreateOrder(OrderCreateRequest request);

        Task<OrderViewModel> GetById(int id);

        Task<bool> DeleteOrder(int id);
    }
}