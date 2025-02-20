using GustoV_Backend.Controllers;
using GustoV_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace GustoV_Backend.Data
{
    public class DataContex : DbContext
    {
        public DataContex(DbContextOptions<DataContex> options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.venta)
                .WithMany(v => v.detalles)
                .HasForeignKey(d => d.ventaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.menu)
                .WithMany()
                .HasForeignKey(d => d.menuId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}