using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class ProductCategoryEditValidator : AbstractValidator<ProductCategoryUpdateRequest>
    {
        public ProductCategoryEditValidator()
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