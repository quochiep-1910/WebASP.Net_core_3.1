using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace eShop.Data.Repositories
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(EShopDbContext EShopDbContext, ILogger<Repository<AppUser>> logger) : base(EShopDbContext, logger)
        {
        }
    }
}
