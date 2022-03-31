using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace eShop.Application.System.Email
{
    public class EmailService : IEmailService
    {
        private readonly AuthMessageSenderOptions options;

        public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public Task Execute(string apiKey, string to, string subject, string html)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(options.EmailFrom),
                Subject = subject,
                PlainTextContent = html,
                HtmlContent = html
            };
            msg.AddTo(new EmailAddress(to));
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }

        public Task SenderEmailAsync(string to, string subject, string html)
        {
            return Execute(options.SendGridKey, to, subject, html);
        }
    }
}