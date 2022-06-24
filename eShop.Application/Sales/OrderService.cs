using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderService(EShopDbContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
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

            if (request.OrderDetails != null)
            {
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
                    await ProductReduction(item.Quantity, item.ProductId);
                    _context.OrderDetails.Add(orderDetails);
                }
                await _context.SaveChangesAsync();
            }
            return order.Id;
        }

        public async Task<int> CreateOrderDetail(List<OrderDetailViewModel> request)
        {
            //2 Save the order detail into Order Detail table
            foreach (var item in request)
            {
                OrderDetail orderDetails = new OrderDetail()
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                await ProductReduction(item.Quantity, item.ProductId);
                _context.OrderDetails.Add(orderDetails);
            }

            await _context.SaveChangesAsync();
            return request.Select(x => x.OrderId).FirstOrDefault();
        }

        /// <summary>
        /// Giảm số lượng sản phẩm nếu mua thành công
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private async Task<int> ProductReduction(int quantity, int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.Stock -= quantity;
            if (product.Stock <= 0)
            {
                product.Stock = 0;
            }
            await _context.SaveChangesAsync();
            return product.Stock;
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
                                      join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                                      join pi in _context.ProductImages on pt.ProductId equals pi.ProductId
                                      where od.OrderId == orderId && pt.LanguageId == "vi-VN"
                                      select new { od, pt, pi }).ToListAsync();
            List<OrderDetailViewModel> orderDetailVN = new List<OrderDetailViewModel>();
            {
                foreach (var item in orderDetails)
                {
                    orderDetailVN.Add(new OrderDetailViewModel()
                    {
                        Price = item.od.Price,
                        ProductId = item.od.ProductId,
                        Quantity = item.od.Quantity,
                        Name = item.pt.Name,
                        Description = item.pt.Description,
                        ThumbnailImage = item.pi.ImagePath
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
                UserId = order.UserId != null ? order.UserId : Guid.Empty.ToString(),
                OrderDetails = orderDetailVN,
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
                    UserId = x.o.UserId,
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

        public async Task<int> GetTotalOrder()
        {
            var totalOrder = await _context.Orders.CountAsync();
            if (totalOrder < 0)
            {
                //cannot find or error
                return 0;
            }
            return totalOrder;
        }

        public async Task<OrderTimeLineViewModel> GetTotalOrderById(string id)
        {
            //var totalOrder = await _context.Orders.Where(x => x.UserId == id)
            //    .Include(x => x.OrderDetails).ToListAsync();
            var totalOrder = await (from o in _context.Orders
                                    join od in _context.OrderDetails on o.Id equals od.OrderId
                                    join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                                    join pi in _context.ProductImages on pt.ProductId equals pi.ProductId
                                    where o.UserId == id && pt.LanguageId == "vi-VN"
                                    select new { o, od, pt, pi }).ToListAsync();
            if (totalOrder.Count() > 0)
            {
                List<OrderDetailTimeLineViewModel> orderDetailVN = new List<OrderDetailTimeLineViewModel>();
                {
                    foreach (var item in totalOrder)
                    {
                        orderDetailVN.Add(new OrderDetailTimeLineViewModel()
                        {
                            UserId = item.o.UserId,
                            ShipName = item.o.ShipName,
                            ShipAddress = item.o.ShipAddress,
                            ShipEmail = item.o.ShipEmail,
                            ShipPhoneNumber = item.o.ShipPhoneNumber,
                            Price = item.od.Price,
                            ProductId = item.od.ProductId,
                            Quantity = item.od.Quantity,
                            Name = item.pt.Name,
                            Description = item.pt.Description,
                            ThumbnailImage = item.pi.ImagePath,
                            OrderId = item.od.OrderId,
                            OrderDate = item.o.OrderDate,
                            Status = (eShop.Utilities.Constants.SystemConstants.OrderStatus)item.o.Status,
                        });
                    }
                };

                var orderViewModel = new OrderTimeLineViewModel()
                {
                    OrderDetails = orderDetailVN
                };

                return orderViewModel;
            }
            return new OrderTimeLineViewModel();
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

        public async Task<bool> ChangeStatusOrder(int orderId, int status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            order.Status = (OrderStatus)status;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}