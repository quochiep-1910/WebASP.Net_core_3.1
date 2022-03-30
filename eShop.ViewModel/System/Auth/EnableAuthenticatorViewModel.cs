using Microsoft.AspNetCore.Mvc;

namespace eShop.ViewModels.System.Auth
{
    public class EnableAuthenticatorViewModel
    {
        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}