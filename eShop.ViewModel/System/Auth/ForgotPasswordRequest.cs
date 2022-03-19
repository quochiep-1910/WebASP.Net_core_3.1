using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.System.Auth
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}