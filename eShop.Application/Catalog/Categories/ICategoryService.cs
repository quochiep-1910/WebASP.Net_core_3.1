using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<ProductCategoryViewModel>> GetAll(string languageId);

        Task<PagedResult<ProductCategoryViewModel>> GetAllPaging(GetManageProductCategoryPagingRequest request);

        Task<ProductCategoryViewModel> GetById(int productCategoryId, string languageId);
    }
}