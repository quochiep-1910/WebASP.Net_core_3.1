using eShop.Application.Catalog.Categories;
using eShop.Application.Catalog.Products;
using eShop.Application.Common;
using eShop.Application.Sales;
using eShop.Application.System.Auth;
using eShop.Application.System.Email;
using eShop.Application.System.Languages;
using eShop.Application.System.Roles;
using eShop.Application.System.Users;
using eShop.Application.Utilities.Slides;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Utilities.Constants;
using eShop.ViewModels.AutoMapper;
using eShop.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.BackendApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //kết nối database
            services.AddDbContext<EShopDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnecttionString)));

            services.AddIdentity<AppUser, IdentityRole<string>>()
              .AddEntityFrameworkStores<EShopDbContext>()
                .AddDefaultTokenProviders();
            //khai báo DI

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();

            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISlideService, SlideService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthService, AuthService>();

            //services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            //services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();

            services.AddAutoMapper(typeof(MapperConfig)); //automapper
            services.AddControllers()
               .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()); //đăng kí tất cả class nào có Validator

            return services;
        }

        public static void ConfigureOptionsPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthMessageSenderOptions>(configuration.GetSection(AuthMessageSenderOptions.EmailSenderSettings));
        }
    }
}