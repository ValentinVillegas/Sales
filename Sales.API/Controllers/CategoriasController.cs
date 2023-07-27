using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var queryable = _context.Categorias;
            return Ok(await queryable.OrderBy(c => c.Nombre).ToListAsync());
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
