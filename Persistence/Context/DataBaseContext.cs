using Application.Interfaces.Contexts;
using Domain.Attributes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Catalogs;
using Persistence.EntityConfigurations;
using Persistence.Seeds;
using Domain.Entities;
using Domain.Order;
using Domain.Payments;

namespace Persistence.Context
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        public DbSet<CatalogBrand> catalogBrands { get; set; }
        public DbSet<CatalogType> catalogTypes { get; set; }
        public DbSet<CatalogItem> catalogItems { get; set; }
        public DbSet<CatalogItemFeature> CatalogItemFeature { get; set; }
        public DbSet<CatalogItemImage> CatalogItemImage { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<UserAddress> userAddresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> payments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AudiTableAttribute), true).Any())
                {
                    builder.Entity(entityType.Name).Property<DateTime>("InsertTime").HasDefaultValue(DateTime.Now);
                    builder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    builder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    builder.Entity(entityType.Name).Property<bool>("IsRemoved").HasDefaultValue(false);
                }
            }
            builder.Entity<CatalogType>()
                .HasQueryFilter(m => EF.Property<bool>(m, "IsRemoved") == false);
            builder.Entity<BasketItem>()
                .HasQueryFilter(m => EF.Property<bool>(m, "IsRemoved") == false);
            builder.Entity<Basket>()
                .HasQueryFilter(m => EF.Property<bool>(m, "IsRemoved") == false);



            builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());

            DataBaseContextSeed.CatalogSeed(builder);
            //
            builder.Entity<Order>().OwnsOne(p => p.Address);
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            var modifedEntity = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added ||
                p.State == EntityState.Modified ||
                p.State == EntityState.Deleted
                );
            foreach (var item in modifedEntity)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                var inserted = entityType.FindProperty("InsertTime");
                var updated = entityType.FindProperty("UpdateTime");
                var removedTime = entityType.FindProperty("RemoveTime");
                var removed = entityType.FindProperty("IsRemoved");
                if (item.State == EntityState.Added && inserted != null)
                {
                    item.Property("InsertTime").CurrentValue = DateTime.Now;
                }
                if (item.State == EntityState.Modified && updated != null)
                {
                    item.Property("UpdateTime").CurrentValue = DateTime.Now;
                }
                //if (item.State == EntityState.Deleted && removedTime != null)
                //{
                //    item.Property("RemoveTime").CurrentValue = DateTime.Now;
                //}
                if (item.State == EntityState.Deleted && removedTime != null && removed != null)
                {
                    item.Property("IsRemoved").CurrentValue = true;
                    item.Property("RemoveTime").CurrentValue = DateTime.Now;
                    item.State = EntityState.Modified;
                }
            }
            return base.SaveChanges();
        }
    }
}
