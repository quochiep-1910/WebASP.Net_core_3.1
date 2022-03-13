using System.Collections.Generic;

namespace eShop.Data.Paging
{
    /// <summary>
    /// A paging response class contains the current page information
    /// and the data inside this page.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingResult<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> Objects { get; set; } = new List<T>();
    }
}
