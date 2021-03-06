using eShop.ViewModels.Common;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using System;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequestrequest);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest userUpdate);

        Task<ApiResult<UserViewModel>> GetById(Guid id);

        Task<UserViewModel> GetByUserName(string userName);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);

        Task<ApiResult<bool>> ChangeUserPassword(AppUserChangePasswordDTO appUserChangePassword);

        Task<bool> ForgotPassword(ForgotPasswordRequest model);

        Task<bool> ResetPassword(ResetPasswordRequest model);

        Task<bool> VerifyEmail(VerifyEmail model);

        #region Two factor Authentication

        Task<TwoFactorAuthenticationViewModel> CheckTwoFactorAuthentication(string userId);

        #endregion Two factor Authentication

        #region Enable Authenticator

        Task<EnableAuthenticatorViewModel> GetEnableAuthenticator(string userId);

        #endregion Enable Authenticator
    }
}