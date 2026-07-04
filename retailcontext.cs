using Microsoft.EntityFrameworkCore;
using RetailManagement.Entities;

namespace RetailManagement.Database
{
    public class RetailContext : DbContext
    {
        public RetailContext(DbContextOptions<RetailContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseLine> PurchaseLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Item -> Department (Many to One)
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Department)
                .WithMany(d => d.Items)
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item -> Vendor (Many to One)
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Vendor)
                .WithMany(v => v.Items)
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseLine -> Purchase
            modelBuilder.Entity<PurchaseLine>()
                .HasOne(pl => pl.Purchase)
                .WithMany(p => p.PurchaseLines)
                .HasForeignKey(pl => pl.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseLine -> Item
            modelBuilder.Entity<PurchaseLine>()
                .HasOne(pl => pl.Item)
                .WithMany()
                .HasForeignKey(pl => pl.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Barcode must be unique
            modelBuilder.Entity<Item>()
                .HasIndex(i => i.Barcode)
                .IsUnique();
        }
    }
}