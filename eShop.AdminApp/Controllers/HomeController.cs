using eShop.AdminApp.Models;
using eShop.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace eShop.AdminApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var ss = HttpContext.Session.GetString("Token");
            var name = User.Identity.Name;
            return View();
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