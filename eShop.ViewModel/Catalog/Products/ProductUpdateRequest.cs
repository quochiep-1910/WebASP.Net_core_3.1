using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }

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

        [Display(Name = "Ảnh thu nhỏ")]
        public IFormFile ThumbnailImage { set; get; }
    }
}