using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.ProductCategory;
using eShop.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;

        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ProductCategoryCreateRequest request)
        {
            var languages = _context.Languages;
            var translations = new List<CategoryTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    //create CategoryTranslations
                    translations.Add(new CategoryTranslation()
                    {
                        Name = request.Name,

                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    translations.Add(new CategoryTranslation()
                    {
                        Name = CategoryConstants.NA,
                        SeoAlias = CategoryConstants.NA,
                        LanguageId = language.Id
                    });
                }
            }
            var category = new Category()
            {
                SortOrder = request.SortOrder,
                IsShowOnHome = request.IsShowOnHome,
                Status = (Status)request.status,
                CategoryTranslations = translations
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null) throw new EShopException($"Không thể tìm thấy một danh mục sản phẩm : {categoryId}");

            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductCategoryViewModel>> GetAll(string languageId)
        {
            //1. Select join
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId

                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Select(x => new ProductCategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId,
                SeoAlias = x.ct.SeoAlias
            }).ToListAsync();
        }

        public async Task<PagedResult<ProductCategoryViewModel>> GetAllPaging(GetManageProductCategoryPagingRequest request)
        {
            //1. Select join
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        //join pic in _context.ProductInCategories on c.Id equals pic.CategoryId

                        where ct.LanguageId == request.LanguageId
                        select new { c, ct };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.ct.Name.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductCategoryViewModel()
                {
                    Id = x.ct.Id,
                    Name = x.ct.Name,
                    CategoryId = x.ct.CategoryId,
                    LanguageId = x.ct.LanguageId,
                    SeoAlias = x.ct.SeoAlias,
                    SeoDescription = x.ct.SeoDescription,
                    SeoTitle = x.ct.SeoTitle,
                    SortOrder = x.c.SortOrder,
                    IsShowOnHome = x.c.IsShowOnHome,
                    ParentId = x.c.ParentId
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductCategoryViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ProductCategoryViewModel> GetById(int CategoryId, string languageId)
        {
            var category = await _context.Categories.FindAsync(CategoryId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == CategoryId
            && x.LanguageId == languageId);

            //mapper
            var productCategoryViewModel = new ProductCategoryViewModel()
            {
                Id = category.Id,
                SortOrder = category.SortOrder,
                IsShowOnHome = category.IsShowOnHome,
                ParentId = category != null ? category.ParentId : null,
                CategoryId = categoryTranslation.CategoryId,
                Name = categoryTranslation.Name,
                SeoDescription = categoryTranslation != null ? categoryTranslation.SeoDescription : null,
                SeoTitle = categoryTranslation != null ? categoryTranslation.SeoTitle : null,
                LanguageId = categoryTranslation.LanguageId,
                SeoAlias = categoryTranslation.SeoAlias,
            };
            return productCategoryViewModel;
        }

        public async Task<int> Update(ProductCategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            var productCategoryTranslations = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId
            && x.LanguageId == request.LanguageId);

            if (category == null || productCategoryTranslations == null)
                throw new EShopException($"Không thể tìm thấy một sản phẩm có id : {request.CategoryId}");

            productCategoryTranslations.Name = request.Name;
            productCategoryTranslations.SeoAlias = request.SeoAlias;
            productCategoryTranslations.SeoDescription = request.SeoDescription;
            productCategoryTranslations.SeoTitle = request.SeoTitle;
            productCategoryTranslations.CategoryId = request.CategoryId;

            category.SortOrder = request.SortOrder;
            category.IsShowOnHome = request.IsShowOnHome;
            category.ParentId = request.ParentId;
            category.Status = (Status)request.status;

            return await _context.SaveChangesAsync();
        }
    }
}