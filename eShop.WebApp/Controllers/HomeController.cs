﻿using eShop.ApiIntegration;
using eShop.WebApp.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IUserApiClient _userApiClient;

        public HomeController(ILogger<HomeController> logger, ISharedCultureLocalizer loc,
            ISlideApiClient slideApiClient, IProductApiClient productApiClient,
            ICategoryApiClient categoryApiClient, IUserApiClient userApiClient)
        {
            _logger = logger;
            _loc = loc;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            var languageId = CultureInfo.CurrentCulture.Name;
            var viewModel = new HomeViewModel
            {
                Slides = await _slideApiClient.GetAll(),
                FeaturedProducts = await _productApiClient
                .GetFeaturedProducts(ProductSettings.NumberOfFeatureProducts, languageId),
                LastestProducts = await _productApiClient
                .GetLatestProducts(ProductSettings.NumberOfLastestProducts, languageId),
                CategoryList = await _categoryApiClient.GetAll(languageId),
            };

            return View(viewModel);
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

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
                );

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = await _userApiClient.GetByUserName(User.Identity.Name);
                var listProductBought = await _productApiClient.GetProductUserBought(userId.Id);
                return View(listProductBought);
            }
            return View();
        }
    }
}