using eShop.AdminApp.Models;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.RevenueStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using static eShop.AdminApp.Models.HomeViewModel;

namespace eShop.AdminApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderService;
        private readonly IContactApiClient _contactApiClient;

        public HomeController(ILogger<HomeController> logger,
            IOrderApiClient orderApiClient, IUserApiClient userApiClient, IProductApiClient productApiClient, IOrderApiClient orderService, IContactApiClient contactApiClient)
        {
            _logger = logger;
            _orderApiClient = orderApiClient;
            _userApiClient = userApiClient;
            _productApiClient = productApiClient;
            _orderService = orderService;
            _contactApiClient = contactApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
                CategoryId = categoryId
            };
            var product = await _productApiClient.GetTopProductSelling(request);
            var optionProductModel = new OptionProductView()
            {
                Items = product.Items,
                Paging = new PagedResultBase()
                {
                    PageIndex = product.PageIndex,
                    PageSize = product.PageSize,
                    TotalRecords = product.TotalRecords
                },
                //PageIndex = product.PageIndex,
                //TotalRecords = product.TotalRecords,
                //PageSize = product.PageSize
            };
            var Revenue = await _orderService.RevenueStatistic(new StatisticsRequest());
            var homeModel = new HomeViewModel()
            {
                TotalOrder = await _orderApiClient.GetTotalOrder(),
                TotalUser = await _userApiClient.GetTotalUser(),
                TotalProducts = await _productApiClient.GetTotalProduct(),
                ListProductTopSelling = optionProductModel,
                RevenueStatisticViews = Revenue,
                TotalContact = await _contactApiClient.GetTotalContact()
            };
            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Language(NavigationViewModel navigationViewModel)
        {
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, navigationViewModel.CurrentLanguageId);

            return Redirect(navigationViewModel.ReturnUrl);
        }
    }
}