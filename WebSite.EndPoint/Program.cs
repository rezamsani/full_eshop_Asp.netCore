using Application.BasketsService;
using Application.Catalogs.GetCatalogIItemPLP;
using Application.Catalogs.GetMenuItem;
using Application.Catalogs.PDP.GetCatalogItemPDP;
using Application.Catalogs.UriComposer;
using Application.Interfaces.Contexts;
using Application.OrderServices;
using Application.PaymentService;
using Application.User;
using Application.Visitors.SaveVisitorInfo;
using Application.Visitors.VisitorOnline;
using Infrastructure.IdentityConfigs;
using Infrastructure.MappingProfile;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Contexts.MongoContext;
using WebSite.EndPoint.Hubs;
using WebSite.EndPoint.Utilities.Filters;
using WebSite.EndPoint.Utilities.Middelware;

namespace WebSite.EndPoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            #region ConctionString
            builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
            builder.Services.AddScoped<IIdentityDataBaseContext, IdentityDataBaseContext>();
            string connectionString = builder.Configuration["ConnectionStrings:SqlServer"];
            builder.Services.AddDbContext<DataBaseContext>(options => { options.UseSqlServer(connectionString); });
            #endregion
            #region ConfigIdentity
            builder.Services.AddIdentityService(builder.Configuration);
            #endregion
            builder.Services.AddAuthorization();
            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.LoginPath = "/Account/login";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.SlidingExpiration = true;
            });
            #region Services
            builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            builder.Services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
            builder.Services.AddTransient<IIVisitorOnlineService, VisitorOnlineService>();
            builder.Services.AddTransient<IGetMenuItemService, GetMenuItemService>();
            builder.Services.AddTransient<IGetCatalogIItemPLPService, GetCatalogIItemPLPService>();
            builder.Services.AddTransient<IUriComposerService, UriComposerService>();
            builder.Services.AddTransient<IGetCatalogItemPDPService, GetCatalogItemPDPService>();
            builder.Services.AddTransient<IBasketsService, BasketsService>();
            builder.Services.AddTransient<IUserAddressServices, UserAddressServices>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IPaymentServices, PaymentServices>();
            builder.Services.AddScoped<SaveVisitorFilter>();
            #endregion
            builder.Services.AddSignalR();
            //mapper
            builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));
            builder.Services.AddAutoMapper(typeof(UserMappingProfile));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSetVisitorId();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<OnlineVisitorHub>("/chathub");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.Run();
        }
    }
}