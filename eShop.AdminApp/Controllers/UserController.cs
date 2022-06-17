using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.AdminApp.Models;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
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
        public async Task<IActionResult> Details(string id)
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
        public async Task<IActionResult> Profile()
        {
            if (User.Identity.Name != null)
            {
                var user = await _userApiClient.GetByUserName(User.Identity.Name);
                return View(user);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdateUser(request.Id, request);

            if (result.IsSuccessed)
            {
                //TempData["result"] = "Cập nhập tài khoản quản trị thành công";
                _notyf.Success("Cập nhập thông tin tài khoản");
                return RedirectToAction("Profile");
            }
            ModelState.AddModelError("", result.Message);//key and message

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
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
        public async Task<IActionResult> Delete(string id)
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

        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(string id)
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

        private async Task<RoleAssignRequest> GetRoleroleAssignRequest(string id)
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
            var authen = await _userApiClient.GetEnableAuthenticator(userId.Id.ToString());
            var result = new EnableAuthenViewModel()
            {
                EnableAuthenticatorViewModel = authen
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenViewModel request)

        {
            var user = await _userApiClient.GetByUserName(User.Identity.Name);
            var resultAuthen = await _userApiClient.PostEnableAuthenticator(request.EnableAuthenticatorRequest, user.Id);
            if (resultAuthen.Message == ResponseMessage.ErrorCodeAuthentication)
            {
                ModelState.AddModelError("", resultAuthen.Message);//key and message
                _notyf.Error(resultAuthen.Message);
                return RedirectToAction("EnableAuthenticator");
            }
            if (resultAuthen.ResultObj.StatusMessage == ResponseMessage.AuthenticatorHasBeenVerified)
            {
                _notyf.Success("Thêm Bảo mật 2 lớp thành công");
                return RedirectToAction("TwoFactorAuthentication");
            }
            else if (resultAuthen.IsSuccessed)
            {
                if (resultAuthen.ResultObj.RecoveryCodes.Length != 0)
                {
                    var parameters = new RouteValueDictionary();
                    for (int i = 0; i < resultAuthen.ResultObj.RecoveryCodes.Length; i++)
                    {
                        parameters["[" + i + "]"] = resultAuthen.ResultObj.RecoveryCodes[i];
                    }

                    return RedirectToAction("ShowRecoveryCodes", parameters);
                }
                _notyf.Success("Thêm Bảo mật 2 lớp thành công");
                return RedirectToAction("TwoFactorAuthentication");
            }
            ModelState.AddModelError("", resultAuthen.Message);//key and message
            _notyf.Error(resultAuthen.Message);
            return RedirectToAction("EnableAuthenticator");
        }

        #endregion Enable Authenticator

        [HttpGet]
        public IActionResult ShowRecoveryCodes(string[] RecoveryCodes)
        {
            var codes = new ShowRecoveryCodes() { RecoveryCodes = RecoveryCodes };

            if (RecoveryCodes == null || RecoveryCodes.Length == 0)
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }
            return View(codes);
        }
    }
}