using AspNetCoreHero.ToastNotification;
using eShop.ApiIntegration;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace eShop.AdminApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string allowSpecificOrigins = "_allowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Notyf
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            services.AddHttpClient();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.AccessDeniedPath = "/User/Forbidden/";
                });

            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter))).AddFluentValidation(); //lọc thông báo hiện lên
            services.AddControllersWithViews()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()); //đăng kí tất cả class nào có Validator;
            services.AddSession(opstions =>
            {
                opstions.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            //DI

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>(); //truy cập HttpContext thông qua  IHttpContextAccessor
            services.AddTransient<IUserApiClient, UserApiClient>();
            services.AddTransient<IRoleApiClient, RoleApiClient>();
            services.AddTransient<ILanguageApiClient, LanguageApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();
            services.AddTransient<IOrderApiClient, OrderApiClient>();
            services.AddCors(options =>

            {
                options.AddPolicy(allowSpecificOrigins,

                builder =>

                {
                    builder.WithOrigins("https://localhost:44380")

                            .AllowAnyHeader()

                            .AllowAnyMethod();
                });
            });
            //biên dịch razor view
            IMvcBuilder builder = services.AddRazorPages();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

#if DEBUG
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif
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
            app.UseStatusCodePages();

            //Other code
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();
            app.UseCors(allowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}