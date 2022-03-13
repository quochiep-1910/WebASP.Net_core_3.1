using AutoMapper;
using eShop.Data.Entities;
using eShop.Data.Paging;
using eShop.ViewModels.Catalog.ProductCategory;

namespace eShop.ViewModels.AutoMapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, ProductCategoryViewModel>().ReverseMap();
            CreateMap<CategoryTranslation, CategoryTranslationViewModel>().ReverseMap();
            CreateMap<CategoryTranslation, ProductCategoryCreateRequest>().ReverseMap();
            CreateMap<Category, ProductCategoryCreateRequest>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            #region PaginatedList

            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListTypeConverter<,>));

            #endregion
        }
    }
}
