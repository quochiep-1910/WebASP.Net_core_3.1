using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Enums;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.ViewModels.Sales.RevenueStatistics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Application.Sales
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderService(EShopDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                UserId = (Guid)order.UserId != null ? (Guid)order.UserId : Guid.Empty,
                OrderDetails = orderDetailVN
            };
            return orderViewModel;
        }

        public async Task<PagedResult<OrderViewModel>> GetOrderPaging(GetOrderPagingRequest request)
        {
            //1.Select
            var order = from o in _context.Orders
                            //join od in _context.OrderDetails on o.Id equals od.OrderId
                        select new { o };

            //2.Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                order = order.Where(x => x.o.ShipName.Contains(request.Keyword) || x.o.ShipPhoneNumber.Contains(request.Keyword));
            }
            //3.Paging
            int totalRow = await order.CountAsync();

            var data = await order.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderViewModel()
                {
                    //order
                    Id = x.o.Id,
                    UserId = (Guid)x.o.UserId,
                    ShipName = x.o.ShipName,
                    ShipAddress = x.o.ShipAddress,
                    ShipEmail = x.o.ShipEmail,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,
                    Status = (eShop.Utilities.Constants.SystemConstants.OrderStatus)x.o.Status,
                    OrderDate = x.o.OrderDate,
                    //OrderDetail
                    //Price = x.od.Price,
                    //ProductId = x.od.ProductId,
                    //Quantity = x.od.Quantity,
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<OrderViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        [Obsolete]
        public async Task<IEnumerable<RevenueStatistic>> GetRevenueStatistic(StatisticsRequest request)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",request.FromDate),
                new SqlParameter("@toDate",request.ToDate)
            };

            //Store
            string StoredProc = "exec GetRevenueStatistic @fromDate,@toDate";

            var result = await _context.RevenueStatistics.FromSql(StoredProc, parameters).ToListAsync();
            List<RevenueStatistic> lst = result.Select(x => new RevenueStatistic
            {
                Benefit = x.Benefit,
                Revenues = x.Revenues,
                Date = x.Date
            }).ToList();
            return result;
        }

        public async Task<int> Update(OrderUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);
            //var orderDetails = await (from o in _context.Orders
            //                          join od in _context.OrderDetails on o.Id equals od.OrderId
            //                          where od.OrderId == request.Id
            //                          select od).ToListAsync();
            if (order == null)
            {
                throw new EShopException($"Không thể tìm thấy một đơn hàng có id : {request.Id}");
            }

            //Update order
            order.ShipName = request.ShipName;
            order.OrderDate = DateTime.Now;
            order.ShipAddress = request.ShipAddress;
            order.ShipEmail = request.ShipEmail;
            order.ShipPhoneNumber = request.ShipPhoneNumber;
            order.Status = (OrderStatus)request.Status;

            ////Update OrderDetail <Feature>
            //if (orderDetails.Count > 0)
            //{
            //    //2 Save the order detail into Order Detail table
            //    foreach (var item in orderDetails)
            //    {
            //        foreach (var res in request.OrderDetails)
            //        {
            //            item.Price = res.Price;
            //            item.ProductId = res.ProductId;
            //            item.Quantity = res.Quantity;
            //        }
            //    }
            //}

            return await _context.SaveChangesAsync();
        }
    }
}