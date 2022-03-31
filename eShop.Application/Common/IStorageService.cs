using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace eShop.Application.Common
{
    public interface IStorageService
    {
        //save file và lấy thông tin file
        string GetFileUrl(string fileName);

        Task<string> SaveFileAsync(IFormFile file);

        Task DeleteFileAsync(string fileName);
    }
}