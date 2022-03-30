using eShop.Application.System.Email;
using eShop.Data.Entities;
using eShop.Utilities.Constants;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,

            RoleManager<AppRole> roleManager,
            IConfiguration config, ILogger<UserService> logger, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _roleManager = roleManager;
            _config = config;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<ApiResult<string>> Authencate(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                //throw new EShopException("Không tìm thấy user name");
                return new ApiErrorResult<string>("Tài Khoản không tồn tại");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return new ApiErrorResult<string>("User account locked out.");
            }
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Đăng nhập không đúng");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim (ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name,loginRequest.UserName)
            };
            //mã hoá claim
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xoá Không Thành công");
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserViewModel>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user); //get roles form database
            var userVM = new UserViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Dob = user.Dob,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                LockoutEnabled = user.LockoutEnabled,
                Roles = roles
            };
            return new ApiSuccessResult<UserViewModel>(userVM);
        }

        public async Task<ApiResult<UserViewModel>> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiErrorResult<UserViewModel>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user); //get roles form database
            var userVM = new UserViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Dob = user.Dob,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                LockoutEnabled = user.LockoutEnabled,
                Roles = roles
            };
            return new ApiSuccessResult<UserViewModel>(userVM);
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    Id = x.Id,
                    LastName = x.LastName,
                    LockoutEnabled = x.LockoutEnabled
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<UserViewModel>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest registerRequest, string origin)
        {
            var user = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (user != null)
            {
                //return false;
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            }
            if (await _userManager.FindByEmailAsync(registerRequest.Email) != null)
            {
                // return false;
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            user = new AppUser()
            {
                Dob = registerRequest.Dob,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.UserName,
                PhoneNumber = registerRequest.PhoneNumber,
                VerificationToken = randomTokenString()
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
            {
                //return true;
                var userFromDb = await _userManager.FindByEmailAsync(registerRequest.Email);
                await sendVerificationEmail(userFromDb, origin);

                return new ApiSuccessResult<bool>();
            }
            //return false;
            return new ApiErrorResult<bool>(result.Errors.FirstOrDefault()?.Description);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                //return false;
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            }
            var removeRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removeRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removeRoles);

            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest registerRequest)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerRequest.Email && x.Id != id))
            {
                //kiểm tra tồn tại
                // return false;
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());

            user.Dob = registerRequest.Dob;
            user.Email = registerRequest.Email;
            user.FirstName = registerRequest.FirstName;
            user.LastName = registerRequest.LastName;
            user.PhoneNumber = registerRequest.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                //return true;
                return new ApiSuccessResult<bool>();
            }
            //return false;
            return new ApiErrorResult<bool>("Cập nhập không thành công");
        }

        public async Task<ApiResult<bool>> ChangeUserPassword(AppUserChangePasswordDTO appUserChangePassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(appUserChangePassword.Id);
                if (user == null)
                    throw new EShopException(ResponseMessage.GetDataFailed);
                var changePass = await _userManager.ChangePasswordAsync(user, appUserChangePassword.OldPasswordHash, appUserChangePassword.PasswordHash);
                if (changePass.Succeeded)
                {     //return true;
                    return new ApiSuccessResult<bool>();
                }
                string.Join(", ", changePass.Errors.Select(x => x.Description));

                //return false;
                return new ApiErrorResult<bool>($"Error:{string.Join(", ", changePass.Errors.Select(x => x.Description))}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} {1}", "Something went wrong in ", nameof(ChangeUserPassword));
                throw;
            }
        }

        public async Task<bool> LockUser(string userid, DateTime? endDate)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                    throw new EShopException(ResponseMessage.FindUserNotFound);
                if (!endDate.HasValue)
                {
                    endDate = DateTime.UtcNow.AddDays(3);
                }
                var lockUserTask = await _userManager.SetLockoutEnabledAsync(user, true);

                var lockDateTask = await _userManager.SetLockoutEndDateAsync(user, endDate);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} {1}", "Something went wrong in ", nameof(LockUser));
                throw;
            }
        }

        public async Task<bool> UnlockUser(string userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                    throw new EShopException(ResponseMessage.FindUserNotFound);
                var lockDisabledTask = await _userManager.SetLockoutEnabledAsync(user, false);

                var setLockoutEndDateTask = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} {1}", "Something went wrong in ", nameof(LockUser));
                throw;
            }
        }

        public async Task<bool> ForgotPassword(string email, string origin)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(email);
                if (appUser == null) throw new EShopException(ResponseMessage.RESOURCE_NOTFOUND(email));
                appUser.ResetToken = randomTokenString();
                appUser.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

                var result = await _userManager.UpdateAsync(appUser);
                if (!result.Succeeded)
                {
                    return false;
                }
                await SendPasswordResetEmail(appUser, origin);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("error ", ex);
                throw;
            }
        }

        public async Task<bool> VerifyEmail(string token)
        {
            try
            {
                if (token == null)
                    throw new Exception("Cannot find user!!");
                var appUserFromDb = await _userManager
                    .Users.Where(x => x.VerificationToken == token).FirstOrDefaultAsync();
                if (appUserFromDb == null) throw new Exception("Verification failed");
                appUserFromDb.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(appUserFromDb);
                if (!result.Succeeded)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error" + ex);
                throw;
            }
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var appUser = await _userManager.Users.Where(x =>
                            x.ResetToken == model.Token
                            && x.ResetTokenExpires > DateTime.UtcNow).SingleOrDefaultAsync();
                if (appUser == null) throw new EShopException(ResponseMessage.RESOURCE_NOTFOUND(model.Email));
                var hasher = new PasswordHasher<AppUser>();
                appUser.PasswordHash = hasher.HashPassword(null, model.Password);
                appUser.PasswordReset = DateTime.UtcNow;
                appUser.ResetToken = null;
                appUser.ResetTokenExpires = null;
                var result = await _userManager.UpdateAsync(appUser);
                if (!result.Succeeded)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("error ", ex);
                throw;
            }
        }

        #region Set up Token

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        #endregion Set up Token

        #region Send Email

        private async Task SendPasswordResetEmail(AppUser appUser, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/Login/ResetPassword?token={appUser.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{appUser.ResetToken}</code></p>";
            }

            await _emailService.SenderEmailAsync(
                to: appUser.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }

        private async Task sendVerificationEmail(AppUser appUser, string origin)
        {
            string msg;
            var token = appUser.VerificationToken;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/Login/VerifyEmail?token={token}";
                msg = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                msg = $@"<p>Please use the below token to verify your email address with the <code>/Auth/verify-email</code> api route:</p>
                             <p><code>{token}</code></p>";
            }

            await _emailService.SenderEmailAsync(
                to: appUser.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {msg}"
            );
        }

        #endregion Send Email
    }
}