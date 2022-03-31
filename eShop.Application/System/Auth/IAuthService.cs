using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShop.Application.System.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Check xem Two factor có đang hoạt động ko
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<TwoFactorAuthenticationViewModel> CheckTwoFactorAuthentication(string userid);

        /// <summary>
        /// Get các mã gen ra qrCode
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<EnableAuthenticatorViewModel> GetEnableAuthenticatorModel(string userid);

        /// <summary>
        /// Post mã từ Authen
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<EnableAuthenticatorViewModel> PostEnableAuthenticatorModel(EnableAuthenticatorRequest request, string userid);
    }
}