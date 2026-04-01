using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //tabla 
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<LotsEntity> Lots { get; set; }
        public DbSet<SaleDetailEntity> SaleDetail { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<VehicleTypeEntity> VehicleTypes { get; set; }
        public DbSet<SocketTypeEntity> SocketTypes { get; set; }


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
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                //entity.Property(e => e.CategoryId)
                //.IsRequired();
                entity.HasOne(p=>p.VehicleType)
                .WithMany()
                .HasForeignKey(p => p.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.VehicleTypeId);
                entity.HasOne(p => p.SocketType)
               .WithMany()
               .HasForeignKey(p => p.SocketTypeId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.SocketTypeId);
                entity.Ignore(e => e.Stock);
                entity.Property(e =>e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                entity.Property(e => e.DeletedAt)
                .IsRequired(false);
                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
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
                    .HasDefaultValueSql("now() at time zone 'utc'");
                    entity.Property<DateTime>("UpdateAt")
                    .IsRequired()
                    .HasDefaultValueSql("now() at time zone 'utc'");

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
                .HasPrecision(18,2)
                .IsRequired();
                entity.HasOne(d => d.Sale)
                .WithMany(s => s.Details)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product)
                .WithMany(p => p.SaleDetails)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.ReturnedQuantity)
                .IsRequired()
                .HasDefaultValue(0);
                entity.Ignore(e => e.Subtotal);
                entity.Ignore(e => e.EffectiveQuantity);
                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");

              
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
                .HasPrecision(18, 2)
                .IsRequired();
                entity.Property(e => e.Status)
                .IsRequired();
                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");

                entity.HasMany(e => e.Details)
                    .WithOne(d => d.Sale)
                    .HasForeignKey(d => d.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Navigation(e => e.Details)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<VehicleTypeEntity>(entity =>
            {
                entity.ToTable("VehicleType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();

               entity.Property(e => e.NameVehicle)
              .IsRequired()
              .HasMaxLength(100);

                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                    .IsRequired()
                    .HasDefaultValueSql("now() at time zone 'utc'");
                entity.Property<DateTime>("UpdateAt")
                    .IsRequired()
                    .HasDefaultValueSql("now() at time zone 'utc'");
            });

            modelBuilder.Entity<SocketTypeEntity>(entity =>
            {
                entity.ToTable("SocketType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.NameSocket)
                    .IsRequired()
                    .HasMaxLength(100);
                //Campos para auditoria
                entity.Property<DateTime>("CreateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
                entity.Property<DateTime>("UpdateAt")
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
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
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
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

