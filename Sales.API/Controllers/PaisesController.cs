using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Sales.API.Data;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/paises")]
    public class PaisesController : ControllerBase
    {
        private readonly DataContext _context;

        public PaisesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Paises.Include(x => x.Estados).OrderBy(x => x.Nombre).ToListAsync());
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetFullAsync()
        {
            return Ok(await _context.Paises.Include(x => x.Estados).ThenInclude(x => x.Municipios).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var pais = await _context.Paises.FirstOrDefaultAsync(x => x.Id == id);

            if(pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

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
