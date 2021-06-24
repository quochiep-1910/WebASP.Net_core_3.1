using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(GetManageProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ProductViewModel> GetById(int id, string languageId);

        Task<ProductImageViewModel> GetImageById(int productId, int imageId);

        Task<List<ProductViewModel>> GetFeaturedProducts(int take, string languageId);

        Task<List<ProductViewModel>> GetLatestProducts(int take, string languageId);

        Task<List<ProductViewModel>> GetRelatedProducts(int take, string languageId);

        Task<bool> UpdateProduct(ProductUpdateRequest productUpdate);

        Task<bool> DeleteProduct(int id);
    }
}