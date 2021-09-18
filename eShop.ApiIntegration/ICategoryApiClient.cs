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
        Task<List<ProductCategoryViewModel>> GetAll(string languageId);

        Task<PagedResult<ProductCategoryViewModel>> GetPagings(GetManageProductCategoryPagingRequest request);

        Task<ProductCategoryViewModel> GetById(int id, string languageId);

        Task<bool> UpdateCategory(ProductCategoryUpdateRequest categoryUpdateRequest);

        Task<bool> CreateCategory(ProductCategoryCreateRequest categoryCreateRequest);

        Task<bool> DeleteCategory(int id);
    }
}