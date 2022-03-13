using eShop.Data.Entities;
using System.Collections.Generic;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.ViewModels.Catalog.ProductCategory
{
    public class CategoryViewModel
    {
        public int Id { set; get; }
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }

        public List<ProductInCategory> ProductInCategories { get; set; }

        public List<CategoryTranslation> CategoryTranslations { get; set; }
    }
}