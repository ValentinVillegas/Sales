using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Sales.Shared.Entidades;

namespace Sales.API.Data
{
    public class DataContext:IdentityDbContext<Usuario>
    {
        public DataContext(DbContextOptions<DataContext> optiones) : base(optiones)
        {

        }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Estado> Estados { get; set; }

        public DbSet<Municipio> Municipios { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<ProductoCategoria> ProductoCategorias { get; set; }

        public DbSet<ProductoImage> ProductoImagenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pais>().HasIndex(x => x.Nombre).IsUnique();
            modelBuilder.Entity<Estado>().HasIndex("PaisId", "Nombre").IsUnique();
            modelBuilder.Entity<Municipio>().HasIndex("EstadoId", "Nombre").IsUnique();
            modelBuilder.Entity<Categoria>().HasIndex(c => c.Nombre).IsUnique();
            modelBuilder.Entity<Producto>().HasIndex(x => x.Nombre).IsUnique();
        }
    }
}