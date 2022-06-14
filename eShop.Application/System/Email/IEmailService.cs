using Microsoft.AspNetCore.Http;
using SendGrid;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.System.Email
{
    public interface IEmailService
    {
        Task SenderEmailAsync(string to, string subject, string html, List<IFormFile> files = null);

        Task<Response> Execute(string apiKey, string to, string subject, string html, List<IFormFile> files = null);
    }
}