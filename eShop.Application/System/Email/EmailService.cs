using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.IO;
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

        public async Task<Response> Execute(string apiKey, string to, string subject, string html, List<IFormFile> files = null)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(options.EmailFrom),
                Subject = subject,
                PlainTextContent = html,
                HtmlContent = html
            };
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FilesSendMail");
            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileNameWithPath = Path.Combine(path, file.FileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileStream = File.OpenRead(fileNameWithPath);
                    await msg.AddAttachmentAsync(file.FileName, fileStream);
                }
            }
            //var fileStream = File.OpenRead("C:\\Users\\BLACKHAT\\Pictures\\eshop\\171480.jpg");
            //await msg.AddAttachmentAsync("gallery1.jpg", fileStream1);

            msg.AddTo(new EmailAddress(to));

            msg.SetClickTracking(false, false);

            return await client.SendEmailAsync(msg);
        }

        public Task SenderEmailAsync(string to, string subject, string html, List<IFormFile> files = null)
        {
            return Execute(options.SendGridKey, to, subject, html, files);
        }
    }
}