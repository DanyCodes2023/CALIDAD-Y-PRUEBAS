using Microsoft.EntityFrameworkCore;
using VENTA_COMPRA.Models;

namespace VENTA_COMPRA.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
                new Rol { RoleID = 1, Description = "Administrador" },
                new Rol { RoleID = 2, Description = "Usuario" }
            );

        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
