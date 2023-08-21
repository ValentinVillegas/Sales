using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Operators;
using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/ordenesTemporales")]

    public class OrdenesTemporalesController : ControllerBase
    {
        private readonly DataContext _context;

        public OrdenesTemporalesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.OrdenesTemporales.Include(c => c.Usuario).Include(c => c.Producto).ThenInclude(p => p.CategoriasProducto!)
                .ThenInclude(cp => cp.Categoria).Include(cp => cp.Producto).ThenInclude(p => p.ProductoImagenes)
                .Where(x => x.Usuario!.Email == User.Identity!.Name).ToListAsync());
        }

        [HttpGet("cantArticulosCarrito")]
        public async Task<IActionResult> GetCantidadArticulos()
        {
            return Ok(await _context.OrdenesTemporales.Where(x => x.Usuario!.Email == User.Identity!.Name).SumAsync(x => x.Cantidad));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _context.OrdenesTemporales.Include(vt => vt.Usuario).Include(vt => vt.Producto)
                .ThenInclude(p => p.CategoriasProducto!).ThenInclude(cp => cp.Categoria).Include(vt => vt.Producto)
                .ThenInclude(p => p.ProductoImagenes).FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(OrdenTemporalDTO ordenTemporalDTO)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == ordenTemporalDTO.IdProducto);

            if (producto is null) return NotFound();

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

            if (usuario is null) return NotFound();

            var temporal = new OrdenTemporal
            {
                Producto = producto,
                Cantidad = ordenTemporalDTO.Cantidad,
                Comentarios = ordenTemporalDTO.Comentarios,
                Usuario = usuario
            };

            try
            {
                _context.OrdenesTemporales.Add(temporal);
                await _context.SaveChangesAsync();
                return Ok(ordenTemporalDTO);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(OrdenTemporalDTO ordenTemporalDTO)
        {
            var ventaTemporal = await _context.OrdenesTemporales.FirstOrDefaultAsync(x => x.Id == ordenTemporalDTO.Id);

            if (ventaTemporal is null) return NotFound();

            ventaTemporal.Comentarios = ordenTemporalDTO.Comentarios;
            ventaTemporal.Cantidad = ordenTemporalDTO.Cantidad;

            _context.OrdenesTemporales.Update(ventaTemporal);
            await _context.SaveChangesAsync();
            return Ok(ventaTemporal);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ventaTemporal = await _context.OrdenesTemporales.FirstOrDefaultAsync(x => x.Id == id);

            if (ventaTemporal is null) return NotFound();

            _context.OrdenesTemporales.Remove(ventaTemporal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
