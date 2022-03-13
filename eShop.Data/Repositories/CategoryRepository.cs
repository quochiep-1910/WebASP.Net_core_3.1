using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using eShop.Data.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(EShopDbContext EShopDbContext, ILogger<Repository<Category>> logger) : base(EShopDbContext, logger)
        {
        }

        public async Task<PaginatedList<CategoryTranslation>> GetAllPaging(string languageId, string keyword, PagingRequest pagingRequest)
        {
            //1. query
            var query = EShopDbContext.CategoryTranslations
                .Where(x => x.LanguageId == languageId)
                .Include(x => x.Category).AsQueryable();
            ;

            //2. Filter
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            return await PaginatedList<CategoryTranslation>.ToPaginatedListAsync(query,
               pagingRequest.PageNumber,
               pagingRequest.PageSize);
        }

        public async Task<List<CategoryTranslation>> GetAll(string languageId)
        {
            var query = await EShopDbContext.CategoryTranslations
                .Where(x => x.LanguageId == languageId)
                .Include(x => x.Category).ToListAsync();

            return query;
        }

        public async Task<List<CategoryTranslation>> GetByCategoryId(int CategoryId, string languageId)
        {
            var query = await EShopDbContext.CategoryTranslations
                .Where(x => x.CategoryId == CategoryId && x.LanguageId == languageId)
                .Include(x => x.Category)
                .ToListAsync()
                ;
            return query;
        }
    }
}