using FluentValidation;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class ProductCategoryCreateValidator : AbstractValidator<ProductCategoryCreateRequest>
    {
        public ProductCategoryCreateValidator()
        {
            //categoryTranslations
            RuleFor(x => x.Name).NotEmpty().WithMessage("Phải nhập tên danh mục sản phẩm");
            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("Phải nhập Từ khoá SEO");
            RuleFor(x => x.SeoDescription).NotEmpty().WithMessage("Phải nhập Mô tả Seo");
            RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("Phải nhập Tiêu đề Seo");

            //category
            RuleFor(x => x.SortOrder).NotEmpty().WithMessage("Phải nhập thứ thự sắp xếp");
        }
    }
}