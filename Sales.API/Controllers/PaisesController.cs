using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/paises")]
    public class PaisesController:ControllerBase
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

        [HttpPost]
        public async Task<ActionResult> PostAsync(Pais pais)
        {
            _context.Add(pais);
            await _context.SaveChangesAsync();
            return Ok(pais);
        }
    }
}
