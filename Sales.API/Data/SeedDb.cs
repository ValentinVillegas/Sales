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
                _context.Paises.Add(new Pais 
                {
                    Nombre = "Estados Unidos",
                    Estados = new List<Estado>()
                    {
                        new Estado()
                        {
                            Nombre = "Florida",
                            Municipios = new List<Municipio>()
                            {
                                new Municipio{Nombre = "Orlando"},
                                new Municipio{Nombre = "Miami"},
                                new Municipio{Nombre = "Tampa"},
                                new Municipio{Nombre = "Fort Lauderdale"},
                                new Municipio{Nombre = "Key West"}
                            }
                        },
                        new Estado()
                        {
                            Nombre = "Texas",
                            Municipios = new List<Municipio>()
                            {
                                new Municipio{Nombre = "Houston"},
                                new Municipio{Nombre = "San Antonio"},
                                new Municipio{Nombre = "Dallas"},
                                new Municipio{Nombre = "Austin"},
                                new Municipio{Nombre = "El Paso"}
                            }
                        }
                    }
                });

                _context.Paises.Add(new Pais
                {
                    Nombre = "México",
                    Estados = new List<Estado>()
                    {
                        new Estado()
                        {
                            Nombre = "Nuevo León",
                            Municipios = new List<Municipio>()
                            {
                                new Municipio{Nombre = "Monterrey"},
                                new Municipio{Nombre = "Guadalupe"},
                                new Municipio{Nombre = "Escobedo"},
                                new Municipio{Nombre = "San Nicolas"},
                                new Municipio{Nombre = "Cadereyta"}
                            }
                        },
                        new Estado()
                        {
                            Nombre = "Jalisco",
                            Municipios = new List<Municipio>()
                            {
                                new Municipio{Nombre = "Jesús María"},
                                new Municipio{Nombre = "Ocotlán"},
                                new Municipio{Nombre = "Puerto Vallarta"},
                                new Municipio{Nombre = "Tequila"},
                                new Municipio{Nombre = "Tonalá"}
                            }
                        }
                    }
                });

                _context.Paises.Add(new Pais
                {
                    Nombre = "Colombia",
                    Estados = new List<Estado>()
                    {
                        new Estado()
                        {
                            Nombre = "Antioquia",
                            Municipios = new List<Municipio>()
                            {
                                new Municipio{Nombre = "Medellín"},
                                new Municipio{Nombre = "Itagüí"},
                                new Municipio{Nombre = "Envigado"},
                                new Municipio{Nombre = "Bello"},
                                new Municipio{Nombre = "Rionegro"}
                            }
                        },
                        new Estado()
                        {
                            Nombre = "Bogota",
                            Municipios = new List<Municipio>()
                            {
                                new Municipio{Nombre = "Usuaquen"},
                                new Municipio{Nombre = "Champinero"},
                                new Municipio{Nombre = "Santa Fe"},
                                new Municipio{Nombre = "Useme"},
                                new Municipio{Nombre = "Bosa"}
                            }
                        }
                    }
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
