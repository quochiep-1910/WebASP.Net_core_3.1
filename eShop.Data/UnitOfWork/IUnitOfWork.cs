using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace eShop.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IContactRepository ContactRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        IOrderRepository OrdersRepository { get; }
        IProductRepository ProductRepository { get; }
        ISlideRepository SlideRepository { get; }
        IUserRepository UserRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveAsync();
    }
}
