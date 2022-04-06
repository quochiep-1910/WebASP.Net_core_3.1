using eShop.ViewModels.Common;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using System;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest loginRequest);

        Task<ApiResult<bool>> Register(RegisterRequest registerRequest, string origin);

        Task<ApiResult<bool>> Update(string id, UserUpdateRequest registerRequest);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<UserViewModel>> GetById(string id);

        /// <summary>
        /// Get Total User Now
        /// </summary>
        /// <returns></returns>
        Task<int> GetToTalUser();

        Task<ApiResult<UserViewModel>> GetByUserName(string userName);

        Task<ApiResult<bool>> Delete(string id);

        Task<ApiResult<bool>> RoleAssign(string id, RoleAssignRequest request);

        #region Identity

        Task<ApiResult<bool>> ChangeUserPassword(AppUserChangePasswordDTO appUserChangePassword);

        Task<bool> LockUser(string userid, DateTime? endDate);

        Task<bool> UnlockUser(string userid);

        Task<bool> ForgotPassword(string email, string origin);

        Task<bool> ResetPassword(ResetPasswordRequest model);

        Task<bool> VerifyEmail(string token);

        #endregion Identity
    }
}