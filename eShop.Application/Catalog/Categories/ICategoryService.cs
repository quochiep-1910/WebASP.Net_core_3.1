using eShop.Data.Paging;
using eShop.ViewModels.Catalog.ProductCategory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryTranslationViewModel>> GetAll(string languageId);

        Task<PagingResult<CategoryTranslationViewModel>> GetAllPaging(string languageId, string keyword, PagingRequest pageQueryParams);

        Task<List<CategoryTranslationViewModel>> GetById(int CategoryId, string languageId);

        Task<int> Update(ProductCategoryUpdateRequest request);

        Task<int> Create(ProductCategoryCreateRequest request);

        Task<int> Delete(int categoryId);
    }
}