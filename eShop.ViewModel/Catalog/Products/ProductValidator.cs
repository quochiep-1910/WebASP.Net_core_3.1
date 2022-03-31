using FluentValidation;

namespace eShop.ViewModels.Catalog.Products
{
    public class ProductValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Price).NotEmpty().WithMessage("Phải nhập Giá");
            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("Phải nhập Giá gốc");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Phải nhập hàng còn lại trong kho");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Phải nhập Tên sản phẩm");
            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("Phải nhập Tiêu đề SEO");
        }
    }
}