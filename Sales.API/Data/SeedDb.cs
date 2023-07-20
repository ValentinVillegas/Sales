using Sales.Shared.Entidades;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPaisesAsync();
        }

        private async Task CheckPaisesAsync()
        {
            if (!_context.Paises.Any())
            {
                _context.Paises.Add(new Pais {Nombre = "Argentina"});
                _context.Paises.Add(new Pais { Nombre = "Perú" });
                _context.Paises.Add(new Pais { Nombre = "Colombia" });
                _context.Paises.Add(new Pais { Nombre = "Brasil" });
                _context.Paises.Add(new Pais { Nombre = "México" });

                await _context.SaveChangesAsync();
            }
        }
    }
}
