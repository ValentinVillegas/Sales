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
            return Ok(await _context.Paises.ToListAsync());
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
            _context.Add(pais);
            await _context.SaveChangesAsync();
            return Ok(pais);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Pais pais)
        {
            _context.Update(pais);
            await _context.SaveChangesAsync();
            return Ok(pais);
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
