using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace eShop.WebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IContactApiClient _contactApiClient;
        private readonly INotyfService _notyf;

        public ContactController(IConfiguration configuration
            , IContactApiClient contactApiClient
            , INotyfService notyfService)
        {
            _configuration = configuration;
            _contactApiClient = contactApiClient;
            _notyf = notyfService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ContactCreateViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _contactApiClient.Create(request);

            if (result)
            {
                _notyf.Success("Tạo contact thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tạo contact thất bại");
            return View(request);
        }
    }
}