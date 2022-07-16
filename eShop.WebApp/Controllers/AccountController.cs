using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShop.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _notyf;

        public AccountController(IUserApiClient userApiClient, IConfiguration configuration, INotyfService notyf)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            var userPrincipal = this.ValidateToken(result.ResultObj);

            //xác thực và thời hạn của phiên đăng nhập
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),

                IsPersistent = false
            };

            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            _notyf.Success($"Xin chào {request.UserName}");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var userId = await _userApiClient.GetByUserName(User.Identity.Name);
            if (userId == null)
            {
                ModelState.AddModelError("", "Không lấy được id");
                return View();
            }
            var user = new AppUserChangePasswordDTO()
            {
                Id = userId.Id
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AppUserChangePasswordDTO request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _userApiClient.ChangeUserPassword(request);
            if (result.IsSuccessed)
            {
                _notyf.Success("Đổi mật khẩu thành công");
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Đổi mật khẩu thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)

                return View(request);
            request.origin = Request.Headers["origin"];
            var result = await _userApiClient.RegisterUser(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var loginResult = await _userApiClient.Authenticate(new LoginRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                RememberMe = true
            });

            var userPrincipal = this.ValidateToken(loginResult.ResultObj);

            //xác thực và thời hạn của phiên đăng nhập
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),

                IsPersistent = false
            };

            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, loginResult.ResultObj);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            _notyf.Success($"Xin chào {request.UserName}");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);
            //model.origin = Request.Headers["origin"];
            model.origin = "https://localhost:44339";
            var result = await _userApiClient.ForgotPassword(model);

            if (result)
            {
                ViewBag.SendEmailSuccess = $"Chúng tôi đã gửi một email tới {model.EmailAddress}";
                return View();
            }
            ModelState.AddModelError("", "Gửi email thất bại");//key and message

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword([FromQuery] string Token)
        {
            if (!ModelState.IsValid)
                return View();

            if (!string.IsNullOrEmpty(Token))
            {
                var tokenReset = new ResetPasswordRequest()
                {
                    Token = Token
                };
                return View(tokenReset);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userApiClient.ResetPassword(model);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Reset mật khẩu thất bại");//key and message

            return View(model);
        }

        //Giải mã Token
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            _notyf.Information("Đăng xuất thành công");
            return RedirectToAction("Index", "Home");
        }
    }
}