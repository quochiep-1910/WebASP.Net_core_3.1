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

namespace eShop.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _notyf;

        public LoginController(IUserApiClient userApiClient, IConfiguration configuration, INotyfService notyf)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            if (result.ResultObj == "RequiresTwoFactor")
            {
                return RedirectToAction("LoginWith2Fa", new { request.RememberMe, request.UserName });
            }

            var userPrincipal = this.ValidateToken(result.ResultObj);

            //xác thực và thời hạn của phiên đăng nhập
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),

                IsPersistent = false
            };
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, _configuration[SystemConstants.AppSettings.DefaultLanguageId]);
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);
            HttpContext!.Response.Cookies.Append("Token", result.ResultObj, cookieOptions);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            _notyf.Success($"Xin chào {request.UserName}");
            return RedirectToAction("Index", "Home");
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
            model.origin = Request.Headers["origin"];
            var result = await _userApiClient.ForgotPassword(model);

            if (result)
            {
                //_notyf.Success($"Đã gửi một email tới {model.EmailAddress}");
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
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Reset mật khẩu thất bại");//key and message

            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyEmail([FromQuery] string Token)
        {
            if (!ModelState.IsValid)
                return View();

            if (!string.IsNullOrEmpty(Token))
            {
                var tokenReset = new VerifyEmail()
                {
                    Token = Token
                };
                return View(tokenReset);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userApiClient.VerifyEmail(model);
            if (result)
            {
                _notyf.Success("Xác thực Email thành công");
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Reset mật khẩu thất bại");//key and message

            return View(model);
        }

        [HttpGet]
        public IActionResult LoginWith2Fa(bool rememberMe, string userName)
        {
            var login2Fa = new LoginWith2fa { RememberMe = rememberMe, UserName = userName };
            return View(login2Fa);
        }

        [HttpPost]
        public async Task<IActionResult> LoginWith2Fa(LoginWith2fa request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _userApiClient.PostLoginWith2Fa(request);

            if (result.IsSuccessed)
            {
                var userPrincipal = this.ValidateToken(result.ResultObj);

                //xác thực và thời hạn của phiên đăng nhập
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),

                    IsPersistent = false
                };
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, _configuration[SystemConstants.AppSettings.DefaultLanguageId]);
                HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);
                HttpContext!.Response.Cookies.Append("Token", result.ResultObj, cookieOptions);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
                _notyf.Success($"Xin chào {request.UserName}");
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = result.Message;
            return RedirectToAction("LoginWith2Fa", new { request.RememberMe, request.UserName });
        }

        [HttpGet]
        public IActionResult Disable2Fa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Disable2Faa()
        {
            var result = await _userApiClient.Disable2Fa();
            if (result.IsSuccessed)
            {
                _notyf.Success("Bạn đã tắt bảo mật thành công");
                return RedirectToAction("TwoFactorAuthentication", "User");
            }
            ModelState.AddModelError("", "Bạn đã tắt bảo mật thất bại");//key and message
            return View();
        }
    }
}