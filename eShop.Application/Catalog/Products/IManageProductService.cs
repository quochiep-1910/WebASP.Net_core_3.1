using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;

using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IManageProductService
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
    }
}