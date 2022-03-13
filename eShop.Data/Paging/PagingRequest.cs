namespace eShop.Data.Paging
{
    /// <summary>
    /// This class is for paging request. Contains page size and current page number.
    /// </summary>
    public class PagingRequest
    {
        public PagingRequest()
        {

        }

        public PagingRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// Default page size value
        /// </summary>
        private const int MaxPageSize = 20;
        /// <summary>
        /// A page number. Default set to page 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// A page size number. Default set to 10.
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// Total amount of items in a page.
        /// If a requested page size exceed the default page size value,
        /// the page size will be set to the default value.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;

            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
