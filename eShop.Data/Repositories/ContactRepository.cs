using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace eShop.Data.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(EShopDbContext EShopDbContext, ILogger<Repository<Contact>> logger) : base(EShopDbContext, logger)
        {
        }
    }
}
