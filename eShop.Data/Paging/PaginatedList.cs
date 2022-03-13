using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Data.Paging
{
    /// <summary>
    /// This class allows data to be separate into multiple pages.
    /// </summary>
    /// <typeparam name="T">A generic type</typeparam>
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// This constructor populates the properties of this class and
        /// add a list of items into an instants of this class.
        /// </summary>
        /// <param name="totalCount">Total items number</param>
        /// <param name="currentPage">Current page number</param>
        /// <param name="pageSize">Current page size</param>
        /// <param name="items">Items to add to this class</param>
        public PaginatedList(int totalCount, int currentPage, int pageSize, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);

            AddRange(items);
        }

        /// <summary>
        /// This method creates a new instant of paginated list
        /// </summary>
        /// <param name="source">A IQueryable of generic type for querying to the DB</param>
        /// <param name="pageNumber">Current requested page number</param>
        /// <param name="pageSize">Current requested page size</param>
        /// <returns>A Paginated list of generic type</returns>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = await source.Skip(
                    (pageNumber - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(count, pageNumber, pageSize, items);
        }
    }
}
