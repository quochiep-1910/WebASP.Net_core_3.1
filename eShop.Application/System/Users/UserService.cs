using eShop.Data.Entities;
using eShop.Utilities.Constants;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Common;
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

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,

            RoleManager<AppRole> roleManager,
            IConfiguration config, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _roleManager = roleManager;
            _config = config;
            _logger = logger;
        }

        public async Task<ApiResult<string>> Authencate(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                //throw new EShopException("Không tìm thấy user name");
                return new ApiErrorResult<string>("Tài Khoản không tồn tại");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, true);
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
                    LastName = x.LastName
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

        public async Task<ApiResult<bool>> Register(RegisterRequest registerRequest)
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
                PhoneNumber = registerRequest.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
            {
                //return true;
                return new ApiSuccessResult<bool>();
            }
            //return false;
            return new ApiErrorResult<bool>("Đăng ký không thành công");
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

        public async Task<bool> ChangeUserPassword(int id, AppUserChangePasswordDTO appUserChangePassword)
        {
            if (id != appUserChangePassword.Id) throw new EShopException(ResponseMessage.NOT_MATCH);
            bool success = false;
            try
            {
                var user = await _userManager.FindByIdAsync(appUserChangePassword.Id.ToString());
                if (user == null)
                    throw new EShopException(ResponseMessage.GetDataFailed);
                var changePass = await _userManager.ChangePasswordAsync(user, appUserChangePassword.OldPasswordHash, appUserChangePassword.PasswordHash);
                if (changePass.Succeeded)
                    success = true;
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} {1}", "Something went wrong in ", nameof(DeleteUser));
                throw;
            }
        }
    }
}