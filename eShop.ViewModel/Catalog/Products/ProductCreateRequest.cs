using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Display(Name = "Giá")]
        public decimal Price { set; get; }

        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }

        [Display(Name = "Hàng trong kho")]
        public int Stock { set; get; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Miêu tả sản phẩm")]
        public string Description { set; get; }

        [Display(Name = "Chi tiết sản phẩm")]
        public string Details { set; get; }

        [Display(Name = "Mô tả Seo ")]
        public string SeoDescription { set; get; }

        [Display(Name = "Tiêu đề Seo ")]
        public string SeoTitle { set; get; }

        [Display(Name = "Từ khoá SEO")]
        public string SeoAlias { get; set; }

        [Display(Name = "Ngôn ngữ")]
        public string LanguageId { set; get; }

        [Display(Name = "Sản phẩm Nổi bật")]
        public bool? IsFeatured { set; get; }

        [Display(Name = "Ảnh thu nhỏ")]
        public IFormFile ThumbnailImage { set; get; }
    }
}