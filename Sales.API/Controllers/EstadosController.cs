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
    [Route("/api/estados")]
    public class EstadosController:ControllerBase
    {
        private readonly DataContext _context;

        public EstadosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Estados.Include(e => e.Municipios).Where(e => e.PaisId == paginacion.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(e => e.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));

            return Ok(await queryable.OrderBy(e => e.Nombre).Paginate(paginacion).ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPaginas([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Estados.Where(e => e.PaisId == paginacion.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(e => e.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));

            double cantidad = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(cantidad / paginacion.RecordsNumber);

            return Ok(totalPaginas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var estado = await _context.Estados.Include(x => x.Municipios).FirstOrDefaultAsync(x => x.Id == id);

            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Estado estado)
        {
            try
            {
                //_context.Add(pais);
                _context.Estados.Add(estado);
                await _context.SaveChangesAsync();
                return Ok(estado);

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un estado con el mismo nombre");
                }

                return BadRequest(dbUpdateException.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Estado estado)
        {
            try
            {
                _context.Update(estado);
                await _context.SaveChangesAsync();
                return Ok(estado);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un estado con el mismo nombre");
                }

                return BadRequest(dbUpdateException.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == id);

            if (estado == null)
            {
                return NotFound();
            }

            _context.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
