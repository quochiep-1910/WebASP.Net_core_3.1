using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;

using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int addedQuantity);

        Task AddViewcount(int productId);

        Task<ProductViewModel> GetById(int productId, string languageId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest productImageViewModel);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest productImageViewModel);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductViewModel>> GetAll(string LanguageId);

        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<List<ProductViewModel>> GetFeatureProducts(string languageId, int take);

        Task<List<ProductViewModel>> GetLatestProducts(string languageId, int take);

        Task<List<ProductViewModel>> GetRelatedProducts(string languageId, int take);

        #region Lịch Công tác(Lâp trình tiên tiến)

        Task<int> CreateWS(WorkingscheduleViewModel request);

        Task<int> UpdateWS(WorkingscheduleViewModel request);

        Task<int> DeleteWS(int requestId);

        Task<WorkingscheduleViewModel> GetByIdWS(int requestId);

        Task<PagedResult<WorkingscheduleViewModel>> GetAllPagingWS(GetUserPagingRequest request);

        #endregion Lịch Công tác(Lâp trình tiên tiến)
    }
}