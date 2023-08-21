using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entidades;
using Sales.Shared.Enums;
using Sales.Shared.Responses;

namespace Sales.API.Helpers
{
    public class OrdenHelper : IOrdenHelper
    {
        private readonly DataContext _context;

        public OrdenHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Response> ProcesarOrdenAsync(string email, string comentarios)
        {
            var usuario = _context.Users.FirstOrDefault(u => u.Email == email);

            if(usuario is null)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = "Usuario inválido"
                };
            }

            var ventasTemporales = await _context.OrdenesTemporales.Include(x => x.Producto ).Where(x => x.Usuario!.Email == email).ToListAsync();

            Response respuesta = await ValidarExistenciaAsync(ventasTemporales);

            if (!respuesta.IsSucces) return respuesta;

            Orden venta = new()
            {
                Fecha = DateTime.UtcNow,
                Usuario = usuario,
                Comentarios = comentarios,
                OrdenDetalles = new List<OrdenDetalle>(),
                OrdenEstatus = OrdenEstatus.Nuevo
            };

            foreach (var ventaTemporal in ventasTemporales)
            {
                venta.OrdenDetalles.Add(new OrdenDetalle 
                { 
                    Producto = ventaTemporal.Producto!,
                    Cantidad = ventaTemporal.Cantidad,
                    Comentarios = ventaTemporal.Comentarios
                });

                Producto? prod = await _context.Productos.FirstOrDefaultAsync(x => x.Id == ventaTemporal.Producto!.Id);

                if (prod is not null)
                {
                    prod.Stock -= ventaTemporal.Cantidad;
                    _context.Productos.Update(prod);
                }

                _context.OrdenesTemporales.Remove(ventaTemporal);
            }

            _context.Ordenes.Add(venta);
            await _context.SaveChangesAsync();
            return respuesta;
        }

        private async Task<Response> ValidarExistenciaAsync(List<OrdenTemporal> ordenesTemporales)
        {
            Response respuesta = new()
            {
                IsSucces = true,
            };

            foreach (var ordenTemporal in ordenesTemporales)
            {
                Producto? prod = await _context.Productos.FirstOrDefaultAsync(x => x.Id == ordenTemporal.Producto!.Id);

                if (prod is null)
                {
                    respuesta.IsSucces = false;
                    respuesta.Message = $"El producto {ordenTemporal.Producto!.Nombre} no se encuentra disponible.";
                    return respuesta;
                }

                if (prod.Stock < ordenTemporal.Cantidad)
                {
                    respuesta.IsSucces = false;
                    respuesta.Message = $"Lo sentimos, el producto {ordenTemporal.Producto!.Nombre} no cuenta con existencia suficiente, intente disminuyendo " +
                        $"la cantidad o sutituyendo el producto por otro";
                    return respuesta;
                }
            }

            return respuesta;
        }
    }
}
