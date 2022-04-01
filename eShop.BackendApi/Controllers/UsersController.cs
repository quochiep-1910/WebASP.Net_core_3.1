using eShop.Application.System.Auth;
using eShop.Application.System.Users;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Authencate(request);
            //result.Message == "RequiresTwoFactor";

            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Register(request, request.origin);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            var products = await _userService.GetUserPaging(request);
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetId")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var user = await _userService.GetByUserName(userName);
            return Ok(user.ResultObj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(string id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RoleAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserPassword(AppUserChangePasswordDTO appUserChangePassword)
        {
            return Ok(await _userService.ChangeUserPassword(appUserChangePassword));
        }

        [HttpPost("LockUser")]
        public async Task<IActionResult> LockUser(string userid, DateTime? endDate)
        {
            return Ok(await _userService.LockUser(userid, endDate));
        }

        [HttpPost("Unlock")]
        public async Task<IActionResult> Unlock(string userid)
        {
            return Ok(await _userService.UnlockUser(userid));
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            return Ok(await _userService.ForgotPassword(model.EmailAddress, model.origin));
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            return Ok(await _userService.ResetPassword(model));
        }

        [HttpGet("VerifyEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            return Ok(await _userService.VerifyEmail(token));
        }

        [HttpGet("CheckTwoFactorAuthentication")]
        public async Task<IActionResult> CheckTwoFactorAuthentication(string userId)
        {
            var result = await _authService.CheckTwoFactorAuthentication(userId);
            return Ok(result);
        }

        [HttpGet("GetEnableAuthenticator")]
        public async Task<IActionResult> GetEnableAuthenticator(string userId)
        {
            var result = await _authService.GetEnableAuthenticatorModel(userId);
            return Ok(result);
        }

        [HttpPost("PostEnableAuthenticator")]
        public async Task<IActionResult> PostEnableAuthenticator(EnableAuthenticatorRequest request, string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.PostEnableAuthenticatorModel(request, userId);
            return Ok(result);
        }
    }
}