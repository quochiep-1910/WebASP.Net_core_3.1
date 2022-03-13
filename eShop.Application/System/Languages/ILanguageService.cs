using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.System.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}