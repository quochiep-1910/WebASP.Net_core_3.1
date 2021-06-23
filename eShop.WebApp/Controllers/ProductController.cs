using eShop.ApiIntegration;

using eShop.ViewModels.Catalog.Products;
using eShop.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Category(int Id, string culture, int page = 1, int pageSize = 10)
        {
            var products = await _productApiClient.GetPagings(new GetManageProductPagingRequest()
            {
                CategoryId = Id,
                PageIndex = page,
                LanguageId = culture,
                PageSize = pageSize
            });
            return View(new CategoryViewModel()
            {
                Category = await _categoryApiClient.GetById(Id, culture),
                Products = products
            });
        }

        public async Task<IActionResult> Detail(int id, string culture)
        {
            var product = await _productApiClient.GetById(id, culture);

            return View(new ProductDetailViewModel()
            {
                Product = product,
                RelatedProducts = await _productApiClient
                 .GetRelatedProducts(ProductSettings.NumberOfRelatedProducts, culture)
            });
        }
    }
}