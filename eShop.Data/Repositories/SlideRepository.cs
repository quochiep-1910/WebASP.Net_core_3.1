using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace eShop.Data.Repositories
{
    public class SlideRepository : Repository<Slide>, ISlideRepository
    {
        public SlideRepository(EShopDbContext EShopDbContext, ILogger<Repository<Slide>> logger) : base(EShopDbContext, logger)
        {
        }
    }
}
