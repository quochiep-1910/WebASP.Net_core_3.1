using AutoMapper;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Paging;
using eShop.Data.UnitOfWork;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.ProductCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;

        public CategoryService(EShopDbContext context, IUnitOfWork unitOfWork,
                             ILogger<CategoryService> logger, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> Create(ProductCategoryCreateRequest request)
        {
            await using var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var languages = await _unitOfWork.LanguageRepository.GetAllAsyncNoPaging();
                var translations = new List<CategoryTranslation>();
                foreach (var language in languages)
                {
                    var categoryTranslation = _mapper.Map<CategoryTranslation>(request);
                    if (language.Id == request.LanguageId)
                    {
                        translations.Add(categoryTranslation);
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
                var category = _mapper.Map<Category>(request);
                category.CategoryTranslations = translations;
                await _context.Categories.AddAsync(category);
                await _unitOfWork.SaveAsync();
                await trans.CommitAsync();
                return category.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Has en error when adding category. {e.InnerException?.Message}");
                await trans.RollbackAsync();
                throw;
            }
        }

        public async Task<int> Delete(int categoryId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category =
                    await _unitOfWork.CategoryRepository.GetAsync(ct => ct.Id == categoryId);
                var result = await _unitOfWork.CategoryRepository.RemoveAsync(category);
                if (result == true)
                {
                    return 1;
                }

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Has en error when delete. {ex.InnerException?.Message}");
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<List<CategoryTranslationViewModel>> GetAll(string languageId)
        {
            try
            {
                var productCategory = _mapper.Map<List<CategoryTranslationViewModel>>(await _unitOfWork.CategoryRepository.GetAll(languageId));

                return productCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<PagingResult<CategoryTranslationViewModel>> GetAllPaging(string languageId, string keyword, PagingRequest pageQueryParams = null)
        {
            try
            {
                var categories = _mapper.Map<PaginatedList<CategoryTranslationViewModel>>
                    (await _unitOfWork.CategoryRepository.GetAllPaging(languageId, keyword, pageQueryParams));

                var pageResult = new PagingResult<CategoryTranslationViewModel>()
                {
                    TotalCount = categories.TotalCount,
                    PageSize = categories.PageSize,
                    TotalPages = categories.TotalPages,
                    CurrentPage = categories.CurrentPage,
                    Objects = categories
                };
                return pageResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<List<CategoryTranslationViewModel>> GetById(int CategoryId, string languageId)
        {
            try
            {
                if (CategoryId != 0 && !string.IsNullOrEmpty(languageId))
                {
                    var category = await _unitOfWork.CategoryRepository.GetByCategoryId(CategoryId, languageId);

                    return _mapper.Map<List<CategoryTranslationViewModel>>(category);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> Update(ProductCategoryUpdateRequest request)
        {
            var category = await _unitOfWork.CategoryRepository.GetByCategoryId(request.CategoryId,);
            var productCategoryTranslations = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId
            && x.LanguageId == request.LanguageId);

            if (category == null || productCategoryTranslations == null)
            {
                throw new EShopException($"Không thể tìm thấy một sản phẩm có id : {request.CategoryId}");
            }

            productCategoryTranslations.Name = request.Name;
            productCategoryTranslations.SeoAlias = request.SeoAlias;
            productCategoryTranslations.SeoDescription = request.SeoDescription;
            productCategoryTranslations.SeoTitle = request.SeoTitle;
            productCategoryTranslations.CategoryId = request.CategoryId;

            category.SortOrder = request.SortOrder;
            category.IsShowOnHome = request.IsShowOnHome;
            category.ParentId = request.ParentId;
            category.Status = request.status;

            return await _context.SaveChangesAsync();
        }
    }
}