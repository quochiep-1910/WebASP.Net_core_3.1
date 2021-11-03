using AspNetCoreHero.ToastNotification;
using eShop.ApiIntegration;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.System.Users;
using eShop.WebApp.LocalizationResources;
using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;

namespace eShop.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            var cultures = new[]
{
            new CultureInfo("en-US"),
            new CultureInfo("vi-VN")
};
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter))).AddFluentValidation(); //lọc thông báo hiện lên
            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()) //đăng kí tất cả class nào có Validator;
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
            {
                // When using all the culture providers, the localization process will
                // check all available culture providers in order to detect the request culture.
                // If the request culture is found it will stop checking and do localization accordingly.
                // If the request culture is not found it will check the next provider by order.
                // If no culture is detected the default culture will be used.

                // Checking order for request culture:
                // 1) RouteSegmentCultureProvider
                //      e.g. http://localhost:1234/tr
                // 2) QueryStringCultureProvider
                //      e.g. http://localhost:1234/?culture=tr
                // 3) CookieCultureProvider
                //      Determines the culture information for a request via the value of a cookie.
                // 4) AcceptedLanguageHeaderRequestCultureProvider
                //      Determines the culture information for a request via the value of the Accept-Language header.
                //      See the browsers language settings

                // Uncomment and set to true to use only route culture provider
                ops.UseAllCultureProviders = false;
                ops.ResourcesPath = "LocalizationResources";
                ops.RequestLocalizationOptions = o =>
                {
                    o.SupportedCultures = cultures;
                    o.SupportedUICultures = cultures;
                    o.DefaultRequestCulture = new RequestCulture("vi-VN");
                };
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = "/Account/Login";
                  options.AccessDeniedPath = "/User/Forbidden/";
              });
            services.AddSession(opstions =>
            {
                opstions.IdleTimeout = TimeSpan.FromMinutes(20);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //truy cập HttpContext thông qua  IHttpContextAccessor
            services.AddTransient<ISlideApiClient, SlideApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();
            services.AddTransient<IUserApiClient, UserApiClient>();
            services.AddTransient<IOrderApiClient, OrderApiClient>();
            //biên dịch razor view
            IMvcBuilder builder = services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseRequestLocalization(); //middleware hộ trợ đa ngôn ngữ
            app.UseSession();
            app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
               name: "Product Category en-US",
               pattern: "{culture}/categories/{id}", new
               {
                   controller = "Product",
                   action = "Category"
               });

            endpoints.MapControllerRoute(
             name: "Product Category vi-VN",
             pattern: "{culture}/danh-muc/{id}", new
             {
                 controller = "Product",
                 action = "Category"
             });

            endpoints.MapControllerRoute(
             name: "Product Detail en-US",
             pattern: "{culture}/products/{id}", new
             {
                 controller = "Product",
                 action = "Detail"
             });

            endpoints.MapControllerRoute(
             name: "Product Detail vi-VN",
             pattern: "{culture}/san-pham/{id}", new
             {
                 controller = "Product",
                 action = "Detail"
             });
            endpoints.MapControllerRoute(
           name: "CheckOut vi-VN",
           pattern: "{culture}/cart/checkout", new
           {
               controller = "Cart",
               action = "Checkout"
           });
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
        });
        }
    }
}