using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;  //truy cập HttpContext thông qua  IHttpContextAccessor

        public ProductApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
               IHttpContextAccessor httpContextAccessor
            ) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateProduct(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "OriginalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "Description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? "" : request.Details.ToString()), "Details");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription.ToString()), "SeoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle.ToString()), "SeoTitle");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(languageId), "LanguageId");

            var response = await client.PostAsync($"/api/products/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<PagedResult<ProductViewModel>> GetPagings(GetManageProductPagingRequest request)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>(
               "/api/products/paging?pageIndex="
                + $"{request.PageIndex}&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&languageId={ request.LanguageId}" +
                $"&categoryId={request.CategoryId}");
            return result;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var json = JsonConvert.SerializeObject(request); //convert json to string
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/products/{id}/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)//kiểm tra status code
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ProductViewModel> GetById(int id, string languageId)
        {
            var data = await GetAsync<ProductViewModel>($"/api/products/{id}/{languageId}");
            return data;
        }

        public async Task<bool> UpdateProduct(ProductUpdateRequest productUpdate)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();
            if (productUpdate.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(productUpdate.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)productUpdate.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage", productUpdate.ThumbnailImage.FileName);
            }
            // requestContent.Add(new StringContent(productUpdate.Id.ToString()), "id");

            requestContent.Add(new StringContent(productUpdate.Price.ToString()), "price");
            requestContent.Add(new StringContent(productUpdate.OriginalPrice.ToString()), "OriginalPrice");
            requestContent.Add(new StringContent(productUpdate.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(productUpdate.IsFeatured.ToString()), "IsFeatured");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(productUpdate.Name) ? "" : productUpdate.Name.ToString()), "Name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(productUpdate.Description) ? "" : productUpdate.Description.ToString()), "Description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(productUpdate.Details) ? "" : productUpdate.Details.ToString()), "Details");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(productUpdate.SeoDescription) ? "" : productUpdate.SeoDescription.ToString()), "SeoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(productUpdate.SeoTitle) ? "" : productUpdate.SeoTitle.ToString()), "SeoTitle");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(productUpdate.SeoAlias) ? "" : productUpdate.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(languageId), "LanguageId");

            var response = await client.PutAsync($"/api/products/" + productUpdate.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(int take, string languageId)
        {
            var data = await GetListAsync<ProductViewModel>($"/api/products/featured/{languageId}/{take}");
            return data;
        }

        public async Task<List<ProductViewModel>> GetLatestProducts(int take, string languageId)
        {
            var data = await GetListAsync<ProductViewModel>($"/api/products/latest/{languageId}/{take}");
            return data;
        }

        public async Task<ProductImageViewModel> GetImageById(int productId, int imageId)
        {
            var data = await GetAsync<ProductImageViewModel>($"/api/products/{productId}/images/{imageId}");
            return data;
        }

        public async Task<List<ProductViewModel>> GetRelatedProducts(int take, string languageId)
        {
            var data = await GetListAsync<ProductViewModel>($"/api/products/related/{languageId}/{take}");
            return data;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await Delete($"/api/products/" + id);
        }

        #region Lập Trình Tiên Tiến. Lịch Công Tác

        public async Task<bool> CreateWorkingSchedule(WorkingscheduleViewModel request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.EndDate.ToString()), "EndDate");
            requestContent.Add(new StringContent(request.StartDate.ToString()), "StartDate");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.UserName) ? "" : request.UserName.ToString()), "UserName");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.LyDo) ? "" : request.LyDo.ToString()), "LyDo");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Message) ? "" : request.Message.ToString()), "Message");

            var response = await client.PostAsync($"/api/products/CreateWs/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<WorkingscheduleViewModel> GetByIdWS(int id)
        {
            var data = await GetAsync<WorkingscheduleViewModel>($"/api/products/FindId/{id}");
            return data;
        }

        public async Task<bool> UpdateWorkingSchedule(WorkingscheduleViewModel workingUpdate)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(workingUpdate.EndDate.ToString()), "EndDate");
            requestContent.Add(new StringContent(workingUpdate.StartDate.ToString()), "StartDate");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(workingUpdate.UserName) ? "" : workingUpdate.UserName.ToString()), "UserName");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(workingUpdate.LyDo) ? "" : workingUpdate.LyDo.ToString()), "LyDo");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(workingUpdate.Message) ? "" : workingUpdate.Message.ToString()), "Message");

            var response = await client.PutAsync($"/api/products/UpdateWs/" + workingUpdate.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteWorkingSchedule(int workingId)
        {
            return await Delete($"/api/products/Delete/" + workingId);
        }

        public async Task<PagedResult<WorkingscheduleViewModel>> GetAll(GetUserPagingRequest request)
        {
            var result = await GetAsync<PagedResult<WorkingscheduleViewModel>>(
                 "/api/products/listWorkingSchedule?pageIndex="
                  + $"{request.PageIndex}&pageSize={request.PageSize}" +
                  $"&keyword={request.Keyword}");
            return result;
        }

        #endregion Lập Trình Tiên Tiến. Lịch Công Tác
    }
}