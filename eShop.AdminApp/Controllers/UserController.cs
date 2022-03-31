using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;
        private readonly INotyfService _notyf;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration,
            IRoleApiClient roleApiClient, INotyfService notyf)
        {
            _roleApiClient = roleApiClient;
            _userApiClient = userApiClient;
            _configuration = configuration;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUsersPagings(request);
            ViewBag.Keyword = keyword;
            // if (TempData["result"] != null)
            // {
            //     ViewBag.SuccessMsg = TempData["result"];
            // }
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);

            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            request.origin = Request.Headers["origin"];
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                //TempData["result"] = "Thêm mới tài khoản quản trị thành công";
                _notyf.Success("Thêm mới tài khoản quản trị thành công");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,

                    Id = id
                };
                return View(updateRequest);//nếu thành công thì load dữ liệu ra
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdateUser(request.Id, request);

            if (result.IsSuccessed)
            {
                //TempData["result"] = "Cập nhập tài khoản quản trị thành công";
                _notyf.Success("Cập nhập tài khoản quản trị thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);//key and message

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(new UserDeleteRequest()
            {
                Id = id,
                UserName = result.ResultObj.UserName,
                Email = result.ResultObj.Email,
                PhoneNumber = result.ResultObj.PhoneNumber
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.Delete(request.Id);

            if (result.IsSuccessed)
            {
                //TempData["result"] = "Xoá tài khoản quản trị thành công";
                _notyf.Success("Xoá tài khoản quản trị thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);//key and message

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleroleAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.RoleAssign(request.id, request);

            if (result.IsSuccessed)
            {
                //TempData["result"] = "Cập nhập quyền thành công";
                _notyf.Success("Cập nhập quyền thành công");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);//key and message

            var roleAssignRequest = await GetRoleroleAssignRequest(request.id);
            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleroleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }

        #region Two factor Authentication

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var userId = await _userApiClient.GetByUserName(User.Identity.Name);
            var resultAuthen = await _userApiClient.CheckTwoFactorAuthentication(userId.Id.ToString());
            return View(resultAuthen);
        }

        #endregion Two factor Authentication

        #region Enable Authenticator

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var userId = await _userApiClient.GetByUserName(User.Identity.Name);
            var resultAuthen = await _userApiClient.GetEnableAuthenticator(userId.Id.ToString());
            return View(resultAuthen);
        }

        #endregion Enable Authenticator
    }
}