using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.ViewModels.Sales.RevenueStatistics;
using eShop.ViewModels.Utilities.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserApiClient _userApiClient;
        private IOrderApiClient _orderApiClient;
        private readonly INotyfService _notyf;

        public OrderController(IOrderApiClient orderService, IConfiguration configuration, INotyfService notyf, IUserApiClient userApiClient)
        {
            _orderApiClient = orderService;
            _configuration = configuration;
            _notyf = notyf;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 8)
        {
            //var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _orderApiClient.GetPagings(request);

            ViewBag.Keyword = keyword;
            TempData["TotalOrder"] = data.TotalRecords;
            return View(data);
        }

        #region CRUD

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userApiClient.GetByUserName(User.Identity.Name);
            var result = new OrderCreateRequest()
            {
                UserId = user.Id
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            int orderId = await _orderApiClient.CreateOrder(request);
            if (orderId != 0)
            {
                return RedirectToAction("CreateOrderDetail", new { orderId = orderId });
            }

            ModelState.AddModelError("", "Thêm đơn hàng thất bại");

            return View(request);
        }

        [HttpGet]
        public IActionResult CreateOrderDetail([FromQuery] int orderId)
        {
            var result = new OrderDetailViewModel()
            {
                OrderId = orderId
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromForm] OrderDetailViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var listOrderDetail = new List<OrderDetailViewModel>();
            listOrderDetail.Add(request);
            var result = await _orderApiClient.CreateOrderDetail(listOrderDetail);
            if (result)
            {
                _notyf.Information("Thêm mới sản phẩm vào đơn hàng thành công");
                return RedirectToAction("CreateOrderDetail", new { orderId = request.OrderId });
            }

            ModelState.AddModelError("", "Thêm mới sản phẩm đơn hàng thất bại");

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderApiClient.GetById(id);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderApiClient.GetById(id);
            //load data
            var editVm = new OrderUpdateRequest()
            {
                Id = order.Id,
                OrderDate = DateTime.Now,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipName = order.ShipName,
                Status = order.Status,
                ShipPhoneNumber = order.ShipPhoneNumber
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] OrderUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _orderApiClient.UpdateOrder(request);
            if (result)
            {
                //TempData["result"] = "Cập nhập sản phẩm thành công";
                _notyf.Success("Cập nhập đơn hàng thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhập đơn hàng thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderApiClient.GetById(id);

            return View(new OrderDeleteRequest()
            {
                Id = id,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipPhoneNumber = order.ShipPhoneNumber
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(OrderDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _orderApiClient.DeleteOrder(request.Id);

            if (result)
            {
                _notyf.Success("Xoá đơn hàng thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xoá đơn hàng không thành công");//key and message
            return View(request);
        }

        #endregion CRUD

        [HttpGet]
        public async Task<IActionResult> PrintOrder(int id)
        {
            var order = await _orderApiClient.GetById(id);
            //load data
            var printData = new OrderViewModel()
            {
                Id = order.Id,
                OrderDate = DateTime.Now,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipName = order.ShipName,
                ShipPhoneNumber = order.ShipPhoneNumber,
                Status = order.Status,
                OrderDetails = order.OrderDetails,
                Price = order.Price,
                Quantity = order.Quantity
            };
            return View(printData);
        }

        #region Revenue

        [HttpGet]
        public async Task<IActionResult> GetRevenue(StatisticsRequest request)
        {
            var Revenue = await _orderApiClient.RevenueStatistic(request);
            //var json = JsonConvert.SerializeObject(Revenue);
            ViewBag.dataSource = Revenue;
            return View(Revenue);
        }

        public class ColumnChartData
        {
            public string x;
            public double yValue;
            public double yValue1;
            public double yValue2;
        }

        #endregion Revenue

        #region Send Email

        [HttpGet]
        public async Task<IActionResult> ComposeEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ComposeEmail(SendMailViewModel sendMailView)
        {
            var result = await _orderApiClient.CreateSendEmail(sendMailView);
            if (result)
            {
                _notyf.Success("Gửi email thành công");
                return RedirectToAction("ComposeEmail");
            }
            return View();
        }

        #endregion Send Email
    }
}