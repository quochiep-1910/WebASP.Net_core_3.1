using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop.Application.Common
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(IWebHostEnvironment webHostEnvironment, ILogger<FileStorageService> logger)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
            _logger = logger;
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        //public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        //{
        //    var filePath = Path.Combine(_userContentFolder, fileName);
        //    using var output = new FileStream(filePath, FileMode.Create);
        //    await mediaBinaryStream.CopyToAsync(output);
        //}
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            try
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
                var filePath = Path.Combine(_userContentFolder, fileName);
                if (!Directory.Exists(_userContentFolder))
                    Directory.CreateDirectory(_userContentFolder);
                using var output = new FileStream(filePath, FileMode.Create);
                await file.OpenReadStream().CopyToAsync(output);
                return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} {1}", "Something went wrong in ", nameof(SaveFileAsync));
                throw;
            }
        }

        public async Task<List<string>> MultipleUpload(List<IFormFile> files)
        {
            try
            {
                var listUrl = new List<string>();
                foreach (var file in files)
                {
                    if (file != null)
                        listUrl.Add(await SaveFileAsync(file));
                }

                return listUrl;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{0} {1}", "Something went wrong in ", nameof(MultipleUpload));
                throw;
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}