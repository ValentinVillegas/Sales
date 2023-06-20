using Microsoft.EntityFrameworkCore;
using Sales.Shared.Entidades;

namespace Sales.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> optiones) : base(optiones)
        {

        }

        public DbSet<Pais> Paises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pais>().HasIndex(x => x.Nombre).IsUnique();
        }
    }
}
