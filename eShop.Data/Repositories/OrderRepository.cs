using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace eShop.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(EShopDbContext EShopDbContext, ILogger<Repository<Order>> logger) : base(EShopDbContext, logger)
        {
        }
    }
}
