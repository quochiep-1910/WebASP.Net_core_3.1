﻿using System.IO;
using System.Threading.Tasks;

namespace eShop.Application.Common
{
    public interface IStorageService
    {
        //save file và lấy thông tin file
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}