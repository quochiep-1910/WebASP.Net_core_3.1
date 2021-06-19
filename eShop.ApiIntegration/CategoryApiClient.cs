using eShop.ViewModels.Catalog.Category;
using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory,
               IConfiguration configuration,
               IHttpContextAccessor httpContextAccessor
            )
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            return await GetListAsync<CategoryViewModel>("/api/categories?languageId=" + languageId);
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
    }
}