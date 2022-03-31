namespace eShop.Application.System.Email
{
    public class AuthMessageSenderOptions
    {
        public const string EmailSenderSettings = "EmailSenderSettings";
        public string SendGridKey { get; set; }
        public string EmailFrom { get; set; }
    }
}