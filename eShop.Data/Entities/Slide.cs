using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.Data.Entities
{
    public class Slide
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public string Description { set; get; }
        public string Image { set; get; }
        public string Url { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}