using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/ventasTemporales")]
    
    public class VentasTemporalesController:ControllerBase
    {
        private readonly DataContext _context;

        public VentasTemporalesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.VentasTemporales.Include(c => c.Usuario).Include(c => c.Producto).ThenInclude(p => p.CategoriasProducto!)
                .ThenInclude(cp => cp.Categoria).Include(cp => cp.Producto).ThenInclude(p => p.ProductoImagenes)
                .Where(x => x.Usuario!.Email == User.Identity!.Name).ToListAsync());
        }

        [HttpGet("cantArticulosCarrito")]
        public async Task<IActionResult> GetCantidadArticulos()
        {
            return Ok(await _context.VentasTemporales.Where(x => x.Usuario!.Email == User.Identity!.Name).SumAsync(x => x.Cantidad));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(VentaTemporalDTO ventaTemporalDTO)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == ventaTemporalDTO.IdProducto);

            if (producto is null) return NotFound();

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

            if (usuario is null) return NotFound();

            var temporal = new VentaTemporal
            {
                Producto = producto,
                Cantidad = ventaTemporalDTO.Cantidad,
                Comentarios = ventaTemporalDTO.Comentarios,
                Usuario = usuario
            };

            try
            {
                _context.VentasTemporales.Add(temporal);
                await _context.SaveChangesAsync();
                return Ok(ventaTemporalDTO);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
