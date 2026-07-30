using lab_integrador.Models;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace lab_integrador.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ManufacturingProcess> ManufacturingProcesses { get; set; }
        public DbSet<OrderProcess> OrderProcesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderProcess>()
                .HasKey(op => new { op.ProductionOrderId, op.ManufacturingProcessId });

            modelBuilder.Entity<OrderProcess>()
                .HasOne(op => op.ProductionOrder)
                .WithMany(o => o.OrderProcesses)
                .HasForeignKey(op => op.ProductionOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProcess>()
                .HasOne(op => op.ManufacturingProcess)
                .WithMany(p => p.OrderProcesses)
                .HasForeignKey(op => op.ManufacturingProcessId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ManufacturingProcess>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }

    }
}
