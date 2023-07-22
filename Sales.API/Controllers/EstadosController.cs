using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/estados")]
    public class EstadosController:ControllerBase
    {
        private readonly DataContext _context;

        public EstadosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Estados.Include(x => x.Municipios).OrderBy(x => x.Nombre).ToListAsync());
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
