using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Enums;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Sales
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;

        public OrderService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(OrderCreateRequest request)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.ShipName);
            if (currentUser != null)
            {
                request.UserId = currentUser.Id;
            }
            var order = new Order()
            {
                //Id = request.Id,
                ShipName = request.ShipName,
                OrderDate = DateTime.Now,
                ShipAddress = request.ShipAddress,
                ShipEmail = request.ShipEmail,
                ShipPhoneNumber = request.ShipPhoneNumber,
                //OrderDetails = orderDetails,
                Status = OrderStatus.InProgress,
                UserId = request.UserId
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //2 Save the order detail into Order Detail table
            foreach (var item in request.OrderDetails)
            {
                OrderDetail orderDetails = new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderDetails.Add(orderDetails);
            }

            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<int> Delete(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new EShopException($"Không thể tìm thấy một đơn đặt hàng: {orderId}");
            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync();
        }

        public Task<List<OrderViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderViewModel> GetById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            var orderDetails = await (from o in _context.Orders
                                      join od in _context.OrderDetails on o.Id equals od.OrderId
                                      where od.OrderId == orderId
                                      select od).ToListAsync();
            List<OrderDetailViewModel> orderDetailVN = new List<OrderDetailViewModel>();
            {
                foreach (var item in orderDetails)
                {
                    orderDetailVN.Add(new OrderDetailViewModel()
                    {
                        Price = item.Price,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
            };

            var orderViewModel = new OrderViewModel()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,

                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipName = order.ShipName,
                ShipPhoneNumber = order.ShipPhoneNumber,
                Status = (eShop.Utilities.Constants.SystemConstants.OrderStatus)order.Status,
                UserId = (Guid)order.UserId,
                OrderDetails = orderDetailVN
            };
            return orderViewModel;
        }
    }
}