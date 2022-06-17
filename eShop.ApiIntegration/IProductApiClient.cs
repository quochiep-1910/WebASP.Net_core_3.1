using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.System.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(GetManageProductPagingRequest request);

        Task<PagedResult<ProductViewModel>> GetTopProductSelling(GetManageProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ProductViewModel> GetById(int id, string languageId);

        Task<OrderViewModel> GetProductUserBought(string userId);

        Task<ProductImageViewModel> GetImageById(int productId, int imageId);

        Task<List<ProductViewModel>> GetFeaturedProducts(int take, string languageId);

        Task<List<ProductViewModel>> GetLatestProducts(int take, string languageId);

        Task<List<ProductViewModel>> GetRelatedProducts(int take, string languageId);

        /// <summary>
        /// Get total products now
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalProduct();

        Task<bool> UpdateProduct(ProductUpdateRequest productUpdate);

        /// <summary>
        /// Add view Count for product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> AddViewCount(int id);

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