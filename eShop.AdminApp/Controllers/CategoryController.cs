using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.ProductCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly INotyfService _notyf;

        public CategoryController(IConfiguration configuration,
            ICategoryApiClient categoryApiClient, INotyfService notyf)
        {
            _categoryApiClient = categoryApiClient;
            _configuration = configuration;
            _notyf = notyf;
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

            // if (TempData["result"] != null)
            // {
            //     ViewBag.SuccessMsg = TempData["result"];
            // }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _categoryApiClient.GetById(id, languageId);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var categories = await _categoryApiClient.GetAll(languageId);

            ViewBag.categories = categories;

            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _categoryApiClient.CreateCategory(request);
            if (result)
            {
                //TempData["result"] = "Thêm mới danh mục sản phẩm thành công";
                _notyf.Success("Thêm mới danh mục sản phẩm thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm danh mục sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var category = await _categoryApiClient.GetById(id, languageId);
            ViewBag.categories = category;
            var editVm = new ProductCategoryUpdateRequest()
            {
                CategoryId = category.Id,
                Name = category.Name,
                SeoAlias = category.SeoAlias,
                SeoDescription = category.SeoDescription,
                SeoTitle = category.SeoTitle,
                SortOrder = category.SortOrder,
                status = (Status)category.status,
                ParentId = category.ParentId,
                IsShowOnHome = category.IsShowOnHome
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductCategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _categoryApiClient.UpdateCategory(request);

            if (result)
            {
                //TempData["result"] = "Cập nhập sản phẩm thành công";
                _notyf.Success("Cập nhập sản phẩm thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhập sản phẩm thất bại");
            return View(request);
        }
    }
}