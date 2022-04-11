using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.AdminApp.Controllers
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

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new ContactPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _contactApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;
            TempData["TotalCategorys"] = data.TotalRecords;
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _contactApiClient.GetById(id);

            return View(result);
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
                _notyf.Success("Thêm mới contact thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm contact thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _contactApiClient.GetById(id);
            ViewBag.categories = contact;
            var editingContact = new ContactViewModel()
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Message = contact.Message,
                Status = (Status)contact.Status
            };
            return View(editingContact);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ContactViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _contactApiClient.Update(request);

            if (result)
            {
                _notyf.Success("Cập nhập contact thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhập contact thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ContactDeleteViewModel()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ContactDeleteViewModel request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _contactApiClient.DeleteContact(request.Id);

            if (result)
            {
                _notyf.Success("Xoá contact thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xoá contact không thành công");//key and message
            return View(request);
        }
    }
}