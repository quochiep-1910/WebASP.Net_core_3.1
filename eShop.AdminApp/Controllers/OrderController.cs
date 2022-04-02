using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.RevenueStatistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IConfiguration _configuration;
        private IOrderApiClient _orderService;
        private readonly INotyfService _notyf;

        public OrderController(IOrderApiClient orderService, IConfiguration configuration, INotyfService notyf)
        {
            _orderService = orderService;
            _configuration = configuration;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 15)
        {
            //var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _orderService.GetPagings(request);

            ViewBag.Keyword = keyword;
            TempData["TotalOrder"] = data.TotalRecords;
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _orderService.CreateOrder(request);
            if (result)
            {
                _notyf.Success("Thêm mới đơn hàng thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm đơn hàng thất bại");

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderService.GetById(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetById(id);
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
            var result = await _orderService.UpdateOrder(request);
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
            var order = await _orderService.GetById(id);

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
            var result = await _orderService.DeleteOrder(request.Id);

            if (result)
            {
                _notyf.Success("Xoá đơn hàng thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xoá đơn hàng không thành công");//key and message
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenue(StatisticsRequest request)
        {
            var Revenue = await _orderService.RevenueStatistic(request);
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
    }
}