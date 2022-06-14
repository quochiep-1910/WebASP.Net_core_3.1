using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eShop.ViewModels.Utilities.Mail
{
    public class SendMailViewModel
    {
        public string ToEmail { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public List<IFormFile> Attachments { get; set; } = null;
    }
}