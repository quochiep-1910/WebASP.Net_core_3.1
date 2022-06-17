using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Contact
{
    public class ContactPagingRequest: PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
