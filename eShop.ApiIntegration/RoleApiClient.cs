using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class RoleApiClient : BaseApiClient, IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;  //truy cập HttpContext thông qua  IHttpContextAccessor

        public RoleApiClient(IHttpClientFactory httpClientFactory,
               IConfiguration configuration,
               IHttpContextAccessor httpContextAccessor
            ) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            var result = await GetAsync<List<RoleViewModel>>($"/api/roles");
            return new ApiSuccessResult<List<RoleViewModel>>(result);
        }
    }
}