using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}