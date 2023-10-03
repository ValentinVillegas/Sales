using Microsoft.EntityFrameworkCore;
using Sales.API.Helpers;
using Sales.API.Services;
using Sales.Shared.Entidades;
using Sales.Shared.Enums;
using Sales.Shared.Responses;
using System.Linq.Expressions;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;
        private readonly IFileStorage _fileStorage;

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper, IFileStorage fileStorage)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            //await _context.Database.EnsureCreatedAsync();
            await CheckCategoriasAsync();
            //await CheckPaisesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Valentin", "Villegas", "valentinvillegas22@yopmail.com", "8114638521", "Calle Luna Calle Sol", "user.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Ledys", "Bedoya", "ledys@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "LedysBedoya.jpeg", UserType.User);
            await CheckUserAsync("3030", "Brad", "Pitt", "brad@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Brad.jpg", UserType.User);
            await CheckUserAsync("4040", "Angelina", "Jolie", "angelina@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Angelina.jpg", UserType.User);
            await CheckUserAsync("5050", "Bob", "Marley", "bob@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "bob.jpg", UserType.User);
            await CheckProductsAsync();
        }

        private async Task CheckCategoriasAsync()
        {
            if (!_context.Categorias.Any())
            {
                List<Categoria> categorias = new List<Categoria>
                {
                    new Categoria { Nombre = "Apple" },
                    new Categoria { Nombre = "Autos" },
                    new Categoria { Nombre = "Belleza" },
                    new Categoria { Nombre = "Calzado" },
                    new Categoria { Nombre = "Comida" },
                    new Categoria { Nombre = "Cosmeticos" },
                    new Categoria { Nombre = "Deportes" },
                    new Categoria { Nombre = "Erótica" },
                    new Categoria { Nombre = "Ferreteria" },
                    new Categoria { Nombre = "Gamer" },
                    new Categoria { Nombre = "Hogar" },
                    new Categoria { Nombre = "Jardín" },
                    new Categoria { Nombre = "Jugetes" },
                    new Categoria { Nombre = "Lenceria" },
                    new Categoria { Nombre = "Mascotas" },
                    new Categoria { Nombre = "Nutrición" },
                    new Categoria { Nombre = "Ropa" },
                    new Categoria { Nombre = "Tecnología" }
                };

                _context.Categorias.AddRange(categorias);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPaisesAsync()
        {
            if (!_context.Paises.Any())
            {
                /*
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
                        }
                    );

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
                        }
                    );

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
                        }
                    );
                }
                await _context.SaveChangesAsync();
                */
                Response responsePaises = await _apiService.GetListAsync<PaisResponse>("/v1", "/countries");
                if (responsePaises.IsSucces)
                {
                    List<PaisResponse> paises = (List<PaisResponse>)responsePaises.Result!;

                    foreach (PaisResponse paisResponse in paises)
                    {
                        Pais pais = await _context.Paises!.FirstOrDefaultAsync(c => c.Nombre == paisResponse.Name!)!;

                        if (pais == null)
                        {
                            pais = new()
                            {
                                Nombre = paisResponse.Name!,
                                Estados = new List<Estado>()
                            };

                            Response responseEstados = await _apiService.GetListAsync<EstadoResponse>("/v1", $"/countries/{paisResponse.Iso2}/states");

                            if (responseEstados.IsSucces)
                            {
                                List<EstadoResponse> estados = (List<EstadoResponse>)responseEstados.Result!;

                                foreach (EstadoResponse estadoResponse in estados)
                                {
                                    Estado estado = pais.Estados!.FirstOrDefault(e => e.Nombre == estadoResponse.Name!)!;
                                    if (estado is null)
                                    {
                                        estado = new()
                                        {
                                            Nombre = estadoResponse.Name!,
                                            Municipios = new List<Municipio>()
                                        };

                                        Response responseMunicipios = await _apiService.GetListAsync<MunicipioResponse>("/v1", $"/countries/{paisResponse.Iso2}/states/{estadoResponse.Iso2}/cities");

                                        if (responseMunicipios.IsSucces)
                                        {
                                            List<MunicipioResponse> municipios = (List<MunicipioResponse>)responseMunicipios.Result!;

                                            foreach (MunicipioResponse municipioResponse in municipios)
                                            {
                                                if (municipioResponse.Name == "Mosfellsbær" || municipioResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }

                                                Municipio municipio = estado.Municipios!.FirstOrDefault(m => m.Nombre == municipioResponse.Name!)!;
                                                if (municipio is null)
                                                {
                                                    estado.Municipios.Add(new Municipio() { Nombre = municipioResponse.Name! });
                                                }
                                            }
                                        }

                                        if (estado.CantidadMunicipios > 0)
                                        {
                                            pais.Estados.Add(estado);
                                        }

                                    }
                                }
                            }

                            if (pais.CantidadEstados > 0)
                            {
                                _context.Paises.Add(pais);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<Usuario> CheckUserAsync(string documento, string nombres, string apellidos, string email, string telefono, string direccion, string image, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);

            if (user is null)
            {
                var municipio = await _context.Municipios.FirstOrDefaultAsync(x => x.Nombre == "Medellín");

                if (municipio is null) municipio = await _context.Municipios.FirstOrDefaultAsync();

                var filePath = $"{Environment.CurrentDirectory}\\Imagenes\\usuarios\\{image}";
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

                user = new Usuario { 
                    Nombre = nombres,
                    Apellido = apellidos,
                    Email = email,
                    UserName = email,
                    PhoneNumber = telefono,
                    Direccion = direccion,
                    Documento = documento,
                    //Municipio = _context.Municipios.FirstOrDefault(),
                    Municipio = municipio,
                    Foto = imagePath,
                    TipoUsuario = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Productos.Any())
            {
                await AgregarProductoAsync("Adidas Barracuda", 270000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "adidas_barracuda.png" });
                await AgregarProductoAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "Adidas_superstar.png" });
                await AgregarProductoAsync("AirPods", 1300000M, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "airpos.png", "airpos2.png" });
                await AgregarProductoAsync("Audifonos Bose", 870000M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "audifonos_bose.png" });
                await AgregarProductoAsync("Bicicleta Ribble", 12000000M, 6F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta_ribble.png" });
                await AgregarProductoAsync("Camisa Cuadros", 56000M, 24F, new List<string>() { "Ropa" }, new List<string>() { "camisa_cuadros.png" });
                await AgregarProductoAsync("Casco Bicicleta", 820000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "casco_bicicleta.png", "casco.png" });
                await AgregarProductoAsync("iPad", 2300000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "ipad.png" });
                await AgregarProductoAsync("iPhone 13", 5200000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
                await AgregarProductoAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "mac_book_pro.png" });
                await AgregarProductoAsync("Mancuernas", 370000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "mancuernas.png" });
                await AgregarProductoAsync("Mascarilla Cara", 26000M, 100F, new List<string>() { "Belleza" }, new List<string>() { "mascarilla_cara.png" });
                await AgregarProductoAsync("New Balance 530", 180000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance530.png" });
                await AgregarProductoAsync("New Balance 565", 179000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance565.png" });
                await AgregarProductoAsync("Nike Air", 233000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_air.png" });
                await AgregarProductoAsync("Nike Zoom", 249900M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_zoom.png" });
                await AgregarProductoAsync("Buso Adidas Mujer", 134000M, 12F, new List<string>() { "Ropa", "Deportes" }, new List<string>() { "buso_adidas.png" });
                await AgregarProductoAsync("Suplemento Boots Original", 15600M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "Boost_Original.png" });
                await AgregarProductoAsync("Whey Protein", 252000M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "whey_protein.png" });
                await AgregarProductoAsync("Arnes Mascota", 25000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "arnes_mascota.png" });
                await AgregarProductoAsync("Cama Mascota", 99000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "cama_mascota.png" });
                await AgregarProductoAsync("Teclado Gamer", 67000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "teclado_gamer.png" });
                await AgregarProductoAsync("Silla Gamer", 980000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "silla_gamer.png" });
                await AgregarProductoAsync("Mouse Gamer", 132000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "mouse_gamer.png" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AgregarProductoAsync(string name, decimal price, float stock, List<string> categorias, List<string> imagenes)
        {
            Producto producto = new()
            {
                Descripcion = name,
                Nombre = name,
                Precio = price,
                Stock = stock,
                CategoriasProducto = new List<ProductoCategoria>(),
                ProductoImagenes = new List<ProductoImage>()
            };

            foreach (var nombreCategoria in categorias)
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == nombreCategoria);
                if (categoria != null)
                {
                    producto.CategoriasProducto.Add(new ProductoCategoria { Categoria = categoria });
                }
            }

            foreach (string? img in imagenes)
            {
                var filePath = $"{Environment.CurrentDirectory}\\Imagenes\\productos\\{img}";
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                producto.ProductoImagenes.Add(new ProductoImage { Image = imagePath });
            }

            _context.Productos.Add(producto);
        }


    }
}
