using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/paises")]
    public class PaisesController : ControllerBase
    {
        private readonly DataContext _context;

        public PaisesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Paises.Include(p => p.Estados).AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(p => p.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));

            return Ok(await queryable.OrderBy(p => p.Nombre).Paginate(paginacion).ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPaginas([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Paises.AsQueryable();
            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(p => p.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));
            double cantidad = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(cantidad / paginacion.RecordsNumber);
            return Ok(totalPaginas);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var pais = await _context.Paises.Include(x => x.Estados).ThenInclude(x => x.Municipios).FirstOrDefaultAsync(x => x.Id == id);

            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        //[HttpGet("full")]
        //public async Task<IActionResult> GetFullAsync()
        //{
        //    return Ok(await _context.Paises.Include(x => x.Estados).ThenInclude(x => x.Municipios).ToListAsync());
        //}

        [HttpPost]
        public async Task<ActionResult> PostAsync(Pais pais)
        {
            try
            {
                //_context.Add(pais);
                _context.Paises.Add(pais);
                await _context.SaveChangesAsync();
                return Ok(pais);

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
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
        public async Task<ActionResult> PutAsync(Pais pais)
        {
            try
            {
                _context.Update(pais);
                await _context.SaveChangesAsync();
                return Ok(pais);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
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
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var pais = await _context.Paises.FirstOrDefaultAsync(x => x.Id == id);

            if (pais == null)
            {
                return NotFound();
            }

            _context.Remove(pais);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
