using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Data.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        //tabla 
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<LotsEntity> Lots { get; set; }
        public DbSet<SaleDetailEntity> SaleDetail { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }

        //sobreescribir un metodo, especificar la estructura 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                //Tabla products
                //Nombre de la tabla
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                //Id obligatorio y autoincrementable
                entity.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                entity.HasIndex(e => e.Sku)
                .IsUnique();
                entity.Property(e => e.Price)
                .IsRequired();
                entity.Property(e => e.CategoryId)
                .IsRequired();
                entity.Ignore(e => e.Stock);

                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE");
            });
                modelBuilder.Entity<LotsEntity>(entity =>
                {
                    //Tabla lots
                    entity.ToTable("Lots");
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id)
                    .IsRequired().ValueGeneratedOnAdd();
                    entity.Property(e => e.ProductId)
                    .IsRequired();
                    entity.Property(e => e.InitialAmount)
                    .IsRequired();
                    entity.Property(e => e.CurrentAmount)
                    .IsRequired();
                    entity.Property(e => e.PurchaseCost)
                    .IsRequired();
                    entity.Property(e => e.ArrivateDate)
                    .IsRequired();
                    entity.Property(e => e.Supplier)
                    .HasMaxLength(200);
                    //Campos para auditoria
                    entity.Property<DateTime>("CreateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE");
                    entity.Property<DateTime>("UpdateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE");

                    //Relación con ProductEntity
                    entity.HasOne(e => e.Product)
                        .WithMany(p => p.Lots)
                        .HasForeignKey(e => e.ProductId)
                        .OnDelete(DeleteBehavior.Cascade);
    
                });
            modelBuilder.Entity<SaleDetailEntity>(entity =>
            {
                //Tabla SaleDetail
                entity.ToTable("SaleDetail");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.SaleId)
                .IsRequired();
                entity.Property(e => e.Quantity)
                .IsRequired();
                entity.Property(e => e.PriceAtSale)
                .IsRequired();
                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE");

                //Relación con ProductEntity
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
    
            });
            modelBuilder.Entity<SaleEntity>(entity =>
            {
                //Tabla Sales
                entity.ToTable("Sales");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Date)
                .IsRequired();
                entity.Property(e => e.Folio)
                .IsRequired();
                entity.Property(e => e.Total)
                .IsRequired();
                entity.Property(e => e.Status)
                .IsRequired();
                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE");

                entity.HasMany(e => e.Details)
                    .WithOne(d => d.Sale)
                    .HasForeignKey(d => d.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ProductEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreateAt").CurrentValue = DateTime.UtcNow;
                }
                entry.Property("UpdateAt").CurrentValue = DateTime.UtcNow;
            }
        }

    }
}

