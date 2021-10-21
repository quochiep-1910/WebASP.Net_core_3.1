using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;  //truy cập HttpContext thông qua  IHttpContextAccessor

        public CategoryApiClient(
               IHttpClientFactory httpClientFactory,
               IConfiguration configuration,
               IHttpContextAccessor httpContextAccessor
            ) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ProductCategoryViewModel>> GetAll(string languageId)
        {
            return await GetListAsync<ProductCategoryViewModel>("/api/categories?languageId=" + languageId);
        }

        public async Task<PagedResult<ProductCategoryViewModel>> GetPagings(GetManageProductCategoryPagingRequest request)
        {
            var result = await GetAsync<PagedResult<ProductCategoryViewModel>>(
                "/api/categories/paging?pageIndex="
                 + $"{request.PageIndex}&pageSize={request.PageSize}" +
                 $"&keyword={request.Keyword}" +
                 $"&languageId={ request.LanguageId}");
            return result;
        }

        public async Task<ProductCategoryViewModel> GetById(int id, string languageId)
        {
            var data = await GetAsync<ProductCategoryViewModel>($"/api/categories/{id}/{languageId}");
            return data;
        }

        public async Task<bool> UpdateCategory(ProductCategoryUpdateRequest categoryUpdateRequest)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(categoryUpdateRequest.SortOrder.ToString()), "SortOrder");
            requestContent.Add(new StringContent(categoryUpdateRequest.status.ToString()), "status");
            requestContent.Add(new StringContent(categoryUpdateRequest.ParentId.ToString()), "ParentId");
            requestContent.Add(new StringContent(categoryUpdateRequest.IsShowOnHome.ToString()), "IsShowOnHome");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryUpdateRequest.Name) ? "" : categoryUpdateRequest.Name.ToString()), "Name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryUpdateRequest.SeoDescription) ? "" : categoryUpdateRequest.SeoDescription.ToString()), "SeoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryUpdateRequest.SeoTitle) ? "" : categoryUpdateRequest.SeoTitle.ToString()), "SeoTitle");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryUpdateRequest.SeoAlias) ? "" : categoryUpdateRequest.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(languageId), "LanguageId");

            var response = await client.PutAsync($"/api/categories/" + categoryUpdateRequest.CategoryId, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateCategory(ProductCategoryCreateRequest categoryCreateRequest)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(categoryCreateRequest.SortOrder.ToString()), "SortOrder");
            requestContent.Add(new StringContent(categoryCreateRequest.IsShowOnHome.ToString()), "IsShowOnHome");
            requestContent.Add(new StringContent(categoryCreateRequest.status.ToString()), "status");
            requestContent.Add(new StringContent(categoryCreateRequest.ParentId.ToString()), "ParentId");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryCreateRequest.Name) ? "" : categoryCreateRequest.Name.ToString()), "Name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryCreateRequest.SeoDescription) ? "" : categoryCreateRequest.SeoDescription.ToString()), "SeoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryCreateRequest.SeoTitle) ? "" : categoryCreateRequest.SeoTitle.ToString()), "SeoTitle");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(categoryCreateRequest.SeoAlias) ? "" : categoryCreateRequest.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(languageId), "LanguageId");

            var response = await client.PostAsync($"/api/categories/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            return await Delete($"/api/categories/" + id);
        }
    }
}