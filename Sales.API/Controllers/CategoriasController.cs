using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAsync([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Categorias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(x => x.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));

            return Ok(await queryable.OrderBy(c => c.Nombre).Paginate(paginacion).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if(categoria is null) return NotFound();

            return Ok(categoria);
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPaginas([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Categorias.AsQueryable();
            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(c => c.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));
            double cantidad = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(cantidad / paginacion.RecordsNumber);
            return Ok(totalPaginas);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre");
                }

                return BadRequest(dbUpdateException.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Categoria categoria)
        {
            try
            {
                _context.Categorias.Update(categoria);
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre");
                }

                return BadRequest(dbUpdateException.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            
            if (categoria is null) return NotFound();

            _context.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
