using System.ComponentModel.DataAnnotations;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class ProductCategoryCreateRequest
    {
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

        [Display(Name = "Trạng Thái kích hoạt")]
        public Status status { set; get; }
    }
}