using eShop.Data.Entities;
using eShop.Data.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Get All Paging
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        Task<PaginatedList<CategoryTranslation>> GetAllPaging(string languageId, string keyword, PagingRequest pagingRequest);

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<CategoryTranslation>> GetAll(string languageId);

        Task<List<CategoryTranslation>> GetByCategoryId(int CategoryId, string languageId);
    }
}