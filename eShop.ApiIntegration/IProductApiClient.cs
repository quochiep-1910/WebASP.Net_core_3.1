using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using System.Collections.Generic;
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

        #region Lập trình tiên tiến

        Task<bool> CreateWorkingSchedule(WorkingscheduleViewModel request);

        Task<WorkingscheduleViewModel> GetByIdWS(int id);

        Task<bool> UpdateWorkingSchedule(WorkingscheduleViewModel workingUpdate);

        Task<bool> DeleteWorkingSchedule(int id);

        Task<PagedResult<WorkingscheduleViewModel>> GetAll(GetUserPagingRequest request);

        #endregion Lập trình tiên tiến
    }
}