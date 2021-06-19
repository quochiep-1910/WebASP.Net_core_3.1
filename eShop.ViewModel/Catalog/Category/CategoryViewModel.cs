using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string SEOAlias { get; set; }
    }
}