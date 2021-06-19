using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class ProductCategoryViewModel
    {
        [Display(Name = "Id")]
        public int Id { set; get; }

        [Display(Name = "CategoryId")]
        public int CategoryId { set; get; }

        [Display(Name = "Tên danh mục sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Mô tả Seo ")]
        public string SeoDescription { set; get; }

        [Display(Name = "Tiêu đề Seo ")]
        public string SeoTitle { set; get; }

        [Display(Name = "Ngôn ngữ")]
        public string LanguageId { set; get; }

        [Display(Name = "Từ khoá SEO")]
        public string SeoAlias { set; get; }

        [Display(Name = "Thứ tự sắp xếp")]
        public int SortOrder { set; get; }

        [Display(Name = "Hiện thị trang chủ")]
        public bool IsShowOnHome { set; get; }

        [Display(Name = "Danh mục cha")]
        public int? ParentId { set; get; }
    }
}