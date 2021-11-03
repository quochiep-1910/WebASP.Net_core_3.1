using System;

namespace eShop.Data.Entities
{
    public class WorkingSchedule
    {
        public int Id { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string LyDo { set; get; }
        public string UserName { set; get; }
        public string Message { set; get; }
    }
}