using Domain.Catalogs;
using Domain.Entities;
using Domain.Order;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        DbSet<CatalogBrand> catalogBrands { get; set; }
        DbSet<CatalogType> catalogTypes { get; set; }
        DbSet<CatalogItem> catalogItems { get; set; }
        DbSet<CatalogItemFeature> CatalogItemFeature { get; set; }
        DbSet<CatalogItemImage> CatalogItemImage { get; set; }
        DbSet<Basket> Baskets { get; set; }
        DbSet<BasketItem> BasketItems { get; set; }
        DbSet<UserAddress> userAddresses { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Payment> payments { get; set; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    }
}
