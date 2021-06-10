using eShop.ViewModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        public int Id { set; get; }

        [Display(Name = "Giá")]
        public decimal Price { set; get; }

        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }

        [Display(Name = "Hàng trong kho")]
        public int Stock { set; get; }

        [Display(Name = "Lượt xem")]
        public int ViewCount { set; get; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { set; get; }

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

        public List<string> Categories { get; set; } = new List<string>();
    }
}