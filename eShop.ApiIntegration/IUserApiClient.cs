using eShop.ViewModels.Common;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequestrequest);

        Task<ApiResult<bool>> UpdateUser(string id, UserUpdateRequest userUpdate);

        Task<ApiResult<UserViewModel>> GetById(string id);

        Task<UserViewModel> GetByUserName(string userName);

        /// <summary>
        /// Get total user now
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalUser();

        Task<ApiResult<bool>> Delete(string id);

        Task<ApiResult<bool>> RoleAssign(string id, RoleAssignRequest request);

        Task<ApiResult<bool>> ChangeUserPassword(AppUserChangePasswordDTO appUserChangePassword);

        Task<bool> ForgotPassword(ForgotPasswordRequest model);

        Task<bool> ResetPassword(ResetPasswordRequest model);

        Task<bool> VerifyEmail(VerifyEmail model);

        #region Two factor Authentication

        Task<TwoFactorAuthenticationViewModel> CheckTwoFactorAuthentication(string userId);

        #endregion Two factor Authentication

        #region Enable Authenticator

        Task<EnableAuthenticatorViewModel> GetEnableAuthenticator(string userId);

        Task<ApiResult<EnableAuthenticatorViewModel>> PostEnableAuthenticator(EnableAuthenticatorRequest request, string userId);

        #endregion Enable Authenticator

        Task<ApiResult<string>> PostLoginWith2Fa(LoginWith2fa request);

        Task<ApiResult<bool>> Disable2Fa();
    }
}