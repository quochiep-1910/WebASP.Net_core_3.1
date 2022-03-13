using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace eShop.Data.Repositories
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(EShopDbContext EShopDbContext, ILogger<Repository<Language>> logger) : base(EShopDbContext, logger)
        {
        }
    }
}
