using eShop.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageViewModel> Languages { get; set; }
        public string CurrentLanguageId { get; set; }
        public string ReturnUrl { get; set; }
    }
}