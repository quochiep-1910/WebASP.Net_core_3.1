using eShop.Application.System.Email;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Utilities.Constants;
using eShop.ViewModels.System.Auth;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace eShop.Application.System.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailService _emailService;
        private readonly UrlEncoder _urlEncoder;
        private readonly EShopDbContext _context;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public AuthService(UserManager<AppUser> userManager, EShopDbContext context,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config, ILogger<AuthService> logger,
            IEmailService emailService, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _config = config;
            _logger = logger;
            _emailService = emailService;
            _urlEncoder = urlEncoder;
        }

        #region Two Factor Authentication

        public async Task<TwoFactorAuthenticationViewModel> CheckTwoFactorAuthentication(string userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString());

                if (user == null)
                {
                    throw new Exception($"Unable to load user.");
                }
                var result = new TwoFactorAuthenticationViewModel();

                #region get info user tokens

                var userTokenKey = await _context.UserTokens
                    .Where(x => x.UserId == new Guid(userid) && x.Name == SystemConstants.UserTokenConstants.AuthenticatorKey)
                    .Select(x => x.Value).FirstOrDefaultAsync() ?? null;
                var valuesRecoveryCode = await _context.UserTokens
                    .Where(x => x.UserId == new Guid(userid) && x.Name == SystemConstants.UserTokenConstants.RecoveryCodes)
                    .Select(x => x.Value).FirstOrDefaultAsync() ?? null;
                var valuesRecovery = new List<string>();
                if (valuesRecoveryCode != null)
                {
                    valuesRecovery = valuesRecoveryCode.Split(';').Select(s => s).ToList();
                }

                #endregion get info user tokens

                if (userTokenKey == null)
                {
                    result.HasAuthenticator = false;
                }
                else
                {
                    result.HasAuthenticator = true;
                }
                result.Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
                result.IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
                result.RecoveryCodesLeft = valuesRecovery.Count();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("error ", ex);
                throw;
            }
        }

        #endregion Two Factor Authentication

        #region Enable AuthenticatorModel

        public async Task<EnableAuthenticatorViewModel> GetEnableAuthenticatorModel(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid.ToString());

            if (user == null)
            {
                throw new Exception("Không thể tải người dùng có ID.");
            }

            return await LoadSharedKeyAndQrCodeUriAsync(userid);
        }

        public async Task<EnableAuthenticatorViewModel> PostEnableAuthenticatorModel(EnableAuthenticatorRequest request, string userid)
        {
            var user = await _userManager.FindByIdAsync(userid.ToString());
            if (user == null)
            {
                throw new Exception("Không thể tải người dùng có ID.");
            }

            //if (!ModelState.IsValid)
            //{
            //    await LoadSharedKeyAndQrCodeUriAsync(user);
            //    return Page();
            //}

            var result = new EnableAuthenticatorViewModel();
            // Strip spaces and hypens
            var verificationCode = request.Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(userid);
                throw new Exception("Mã xác minh không hợp lệ.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("Người dùng đã bật 2FA với một ứng dụng xác thực.", userId);

            result.StatusMessage = "Ứng dụng xác thực của bạn đã được xác minh.";

            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                result.RecoveryCodes = recoveryCodes.ToArray();

                //return {Show Recovery Codes}
                return result;
            }
            else
            {
                //success
                return result;
            }
        }

        private async Task<EnableAuthenticatorViewModel> LoadSharedKeyAndQrCodeUriAsync(string userid)
        {
            // Load the authenticator key & QR code URI to display on the form

            var user = await _userManager.FindByIdAsync(userid);
            var unformattedKey = await _context.UserTokens
                .Where(x => x.UserId == new Guid(userid)).Select(x => x.Value).FirstOrDefaultAsync() ?? null;
            if (string.IsNullOrEmpty(unformattedKey))
            {
                unformattedKey = _userManager.GenerateNewAuthenticatorKey();

                var userToken = new IdentityUserToken<Guid>()
                {
                    LoginProvider = SystemConstants.UserTokenConstants.AspNetUserStore,
                    Value = unformattedKey,
                    Name = SystemConstants.UserTokenConstants.AuthenticatorKey,
                    UserId = user.Id
                };
                await _context.UserTokens.AddAsync(userToken);
                await _context.SaveChangesAsync();
            }
            var result = new EnableAuthenticatorViewModel();
            result.SharedKey = FormatKey(unformattedKey);

            var email = await _userManager.GetEmailAsync(user);
            result.AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);

            return result;
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("eShop.BackendApi"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        #endregion Enable AuthenticatorModel
    }
}