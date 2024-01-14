using Admin.EndPoint.MappingProfiles;
using Application.Catalogs.AddNewCatalogItem;
using Application.Catalogs.CatalogItemServices;
using Application.Catalogs.CatalogTypeCrud;
using Application.Interfaces.Contexts;
using Application.Visitors.GetTodayReportService;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.ExternalApi.ImageServer;
using Infrastructure.MappingProfile;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Contexts.MongoContext;

namespace Admin.EndPoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddTransient<IGetTodayReportService, GetTodayReportService>();
            builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            builder.Services.AddTransient<ICatalogTypeService, CatalogTypeService>();
            builder.Services.AddTransient<IAddNewCatalogItemService, AddNewCatalogItemService>();
            builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
            builder.Services.AddTransient<IImageUploadService, ImageUploadService>();
            #region ConctionString
            builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
            string connectionString = builder.Configuration["ConnectionStrings:SqlServer"];
            builder.Services.AddDbContext<DataBaseContext>(options => { options.UseSqlServer(connectionString); });
            #endregion
            //mapper
            builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));
            builder.Services.AddAutoMapper(typeof(CatalogVMMappingProfile));
            //fluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddTransient<IValidator<AddNewCatalogItemDto>,AddNewCatalogItemDtoValidator>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}