using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace eShop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(EShopDbContext EShopDbContext, ILogger<Repository<Product>> logger) : base(EShopDbContext, logger)
        {
        }
    }
}
