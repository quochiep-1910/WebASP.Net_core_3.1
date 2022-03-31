using eShop.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(IHttpClientFactory httpClientFactory,
               IConfiguration configuration,
               IHttpContextAccessor httpContextAccessor
            )
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            return await GetListAsync<SlideViewModel>("/api/slides");
        }
    }
}