using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.ProductCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryController(IConfiguration configuration,
            ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetManageProductCategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId
            };
            var data = await _categoryApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(languageId);

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _categoryApiClient.GetById(id, languageId);

            return View(result);
        }
    }
}