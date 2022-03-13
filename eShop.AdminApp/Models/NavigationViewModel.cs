using eShop.ViewModels.System.Languages;
using System.Collections.Generic;

namespace eShop.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageViewModel> Languages { get; set; }
        public string CurrentLanguageId { get; set; }
        public string ReturnUrl { get; set; }
    }
}