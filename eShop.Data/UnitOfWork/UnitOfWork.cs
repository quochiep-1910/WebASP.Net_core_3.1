using eShop.Data.EF;
using eShop.Data.Interfaces;
using eShop.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace eShop.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Attributes
        private readonly EShopDbContext _context;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private ISlideRepository _slideRepository;
        private ILanguageRepository _languageRepository;
        private IContactRepository _contactRepository;
        #endregion

        #region Logger
        private readonly ILogger<CategoryRepository> _categoryLogger;
        private readonly ILogger<ProductRepository> _productLogger;
        private readonly ILogger<OrderRepository> _orderLogger;
        private readonly ILogger<SlideRepository> _slideLogger;
        private readonly ILogger<ContactRepository> _contactLogger;
        private readonly ILogger<LanguageRepository> _languageLogger;
        private readonly ILogger<UserRepository> _userLogger;


        #endregion



        public UnitOfWork(ILogger<CategoryRepository> categoryLogger,
            EShopDbContext context, ILogger<ProductRepository> productLogger, ILogger<OrderRepository> orderLogger, ILogger<SlideRepository> slideLogger, ILogger<ContactRepository> contactLogger, ILogger<LanguageRepository> languageLogger, ILogger<UserRepository> userLogger)
        {
            _categoryLogger = categoryLogger;
            _context = context;
            _productLogger = productLogger;
            _orderLogger = orderLogger;
            _slideLogger = slideLogger;
            _contactLogger = contactLogger;
            _languageLogger = languageLogger;
            _userLogger = userLogger;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.CurrentTransaction ?? await _context.Database.BeginTransactionAsync();
        }

        public ICategoryRepository CategoryRepository =>
           _categoryRepository ??= new CategoryRepository(_context, _categoryLogger);

        public IContactRepository ContactRepository => _contactRepository ??= new ContactRepository(_context, _contactLogger);

        public ILanguageRepository LanguageRepository => _languageRepository ??= new LanguageRepository(_context, _languageLogger);

        public IOrderRepository OrdersRepository => _orderRepository ??= new OrderRepository(_context, _orderLogger);

        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context, _productLogger);

        public ISlideRepository SlideRepository => _slideRepository ??= new SlideRepository(_context, _slideLogger);

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _userLogger);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
