using System;

namespace eShop.ViewModels.Common
{
    public class PagedResultBase
    {
        public int PageIndex { set; get; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
}