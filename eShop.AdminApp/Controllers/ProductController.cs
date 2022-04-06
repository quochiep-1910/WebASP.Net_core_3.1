using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly INotyfService _notyf;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration,
            ICategoryApiClient categoryApiClient, INotyfService notyf)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _configuration = configuration;
            _notyf = notyf;
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
            var data = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(languageId);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            // if (TempData["result"] != null)
            // {
            //     ViewBag.SuccessMsg = TempData["result"];
            // }
            TempData["TotalProducts"] = data.TotalRecords;

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                //TempData["result"] = "Thêm mới sản phẩm thành công";
                _notyf.Success("Thêm mới sản phẩm thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.CategoryAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                //TempData["result"] = "Cập nhập danh mục thành công";
                _notyf.Success("Cập nhập danh mục thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);//key and message
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);
            return View(roleAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var productOjb = await _productApiClient.GetById(id, languageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productOjb.Categories.Contains(category.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _productApiClient.GetById(id, languageId);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var product = await _productApiClient.GetById(id, languageId);

            var editVm = new ProductUpdateRequest()
            {
                Id = product.Id,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Stock = product.Stock,
                IsFeatured = product.IsFeatured,
                ThumbnailImage = Base64ToImage(product.ThumbnailImage, product.Name)
            };

            return View(editVm);
        }

        /// <summary>
        /// Convert IFormFile to string
        /// </summary>
        /// <param name="equipmentFiles"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private IFormFile Base64ToImage(string equipmentFiles, string name)
        {
            IFormFile file = new FormFile(null, 0, 1, name, equipmentFiles);

            return file;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                //TempData["result"] = "Cập nhập sản phẩm thành công";
                _notyf.Success("Cập nhập sản phẩm thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhập sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProductDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.DeleteProduct(request.Id);

            if (result)
            {
                TempData["result"] = "Xoá sản phẩm thành công";
                _notyf.Success("Xoá sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xoá sản phẩm không thành công");//key and message
            return View(request);
        }

        #region lập trình tiên tiến

        public async Task<IActionResult> IndexWS(string keyword, int pageIndex = 1, int pageSize = 3)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.Keyword = keyword;

            var categories = await _productApiClient.GetAll(request);

            return View(categories);
        }

        [HttpGet("CreateWS")]
        public IActionResult CreateWS()
        {
            return View();
        }

        [HttpPost("CreateWS")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateWS([FromForm] WorkingscheduleViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.CreateWorkingSchedule(request);
            if (result)
            {
                //TempData["result"] = "Thêm mới sản phẩm thành công";
                _notyf.Success("Thêm mới lịch công tác thành công");
                return RedirectToAction("IndexWS");
            }

            ModelState.AddModelError("", "Thêm lịch công tác thất bại");

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWS(int id)
        {
            var ws = await _productApiClient.GetByIdWS(id);
            return View(new WorkingscheduleViewModel()
            {
                Id = ws.Id,
                UserName = ws.UserName,
                EndDate = ws.EndDate,
                StartDate = ws.StartDate,
                LyDo = ws.LyDo,
                Message = ws.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWS(WorkingscheduleViewModel request)
        {
            var result = await _productApiClient.DeleteWorkingSchedule(request.Id);

            if (result)
            {
                TempData["result"] = "Xoá lịch công tác thành công";
                _notyf.Success("Xoá lịch công tác thành công");
                return RedirectToAction("IndexWS");
            }
            ModelState.AddModelError("", "Xoá lịch công tác không thành công");//key and message
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> EditWS(int id)
        {
            var ws = await _productApiClient.GetByIdWS(id);

            var workingscheduleVm = new WorkingscheduleViewModel()
            {
                Id = ws.Id,
                UserName = ws.UserName,
                EndDate = ws.EndDate,
                StartDate = ws.StartDate,
                LyDo = ws.LyDo,
                Message = ws.Message
            };
            return View(workingscheduleVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditWS([FromForm] WorkingscheduleViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.UpdateWorkingSchedule(request);
            if (result)
            {
                //TempData["result"] = "Cập nhập lịch công tác thành công";
                _notyf.Success("Cập nhập lịch công tác thành công");
                return RedirectToAction("IndexWS");
            }

            ModelState.AddModelError("", "Cập nhập lịch công tác thất bại");
            return View(request);
        }

        #endregion lập trình tiên tiến
    }
}