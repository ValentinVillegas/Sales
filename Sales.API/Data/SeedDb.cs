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

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriasAsync();
            await CheckPaisesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Valentin", "Villegas", "valentinvillegas22@yopmail.com", "8114638521", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task CheckCategoriasAsync()
        {
            if (!_context.Categorias.Any())
            {
                List<Categoria> categorias = new List<Categoria>() {
                    new Categoria { Nombre = "Juegueteríaa"},
                    new Categoria { Nombre = "Ferretería"},
                    new Categoria { Nombre = "Electrodomésticos"},
                    new Categoria { Nombre = "Ropa"},
                    new Categoria { Nombre = "Deportes"},
                    new Categoria { Nombre = "Abarrotes"},
                    new Categoria { Nombre = "Electrónica"},
                    new Categoria { Nombre = "Linea Blanca"},
                    new Categoria { Nombre = "Salud"},
                    new Categoria { Nombre = "Panadería"},
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

        private async Task<Usuario> CheckUserAsync(string documento, string nombres, string apellidos, string email, string telefono, string direccion, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);

            if (user is null)
            {
                user = new Usuario { 
                    Nombre = nombres,
                    Apellido = apellidos,
                    Email = email,
                    UserName = email,
                    PhoneNumber = telefono,
                    Direccion = direccion,
                    Documento = documento,
                    Municipio = _context.Municipios.FirstOrDefault(),
                    TipoUsuario = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }
    }
}
