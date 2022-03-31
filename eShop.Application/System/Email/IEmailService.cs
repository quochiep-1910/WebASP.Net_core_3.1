using System.Threading.Tasks;

namespace eShop.Application.System.Email
{
    public interface IEmailService
    {
        Task SenderEmailAsync(string to, string subject, string html);

        Task Execute(string apiKey, string to, string subject, string html);
    }
}