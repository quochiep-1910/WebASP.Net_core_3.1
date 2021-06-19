using eShop.ViewModels.Catalog.Category;
using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);

        Task<PagedResult<ProductCategoryViewModel>> GetPagings(GetManageProductCategoryPagingRequest request);

        Task<ProductCategoryViewModel> GetById(int id, string languageId);
    }
}