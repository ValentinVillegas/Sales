using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/municipios")]
    public class MunicipiosController:ControllerBase
    {
        private readonly DataContext _context;

        public MunicipiosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Municipios.OrderBy(x => x.Nombre).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var municipio = await _context.Municipios.FirstOrDefaultAsync(x => x.Id == id);

            if (municipio == null)
            {
                return NotFound();
            }

            return Ok(municipio);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Municipio municipio)
        {
            try
            {
                //_context.Add(pais);
                _context.Municipios.Add(municipio);
                await _context.SaveChangesAsync();
                return Ok(municipio);

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un municipio con el mismo nombre");
                }

                return BadRequest(dbUpdateException.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Municipio municipio)
        {
            try
            {
                _context.Update(municipio);
                await _context.SaveChangesAsync();
                return Ok(municipio);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un municipio con el mismo nombre");
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
            var municipio = await _context.Municipios.FirstOrDefaultAsync(x => x.Id == id);

            if (municipio == null)
            {
                return NotFound();
            }

            _context.Remove(municipio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
