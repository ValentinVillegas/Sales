using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;
using Sales.Shared.Enums;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/ordenes")]
    public class OrdenesController:ControllerBase
    {
        private readonly IOrdenHelper _ordenHelper;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrdenesController(IOrdenHelper ordenHelper, DataContext context, IUserHelper userHelper)
        {
            _ordenHelper = ordenHelper;
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO paginacion)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            if (usuario is null) return BadRequest("Usuario no válido");

            var queryable = _context.Ordenes.Include(o => o.Usuario).Include(o => o.OrdenDetalles!).ThenInclude(o => o.Producto).AsQueryable();

            var esAdministrador = await _userHelper.IsUserInRoleAsync(usuario, UserType.Admin.ToString());

            if (!esAdministrador) queryable = queryable.Where(o => o.Usuario!.Email == User.Identity!.Name);

            return Ok(await queryable.OrderByDescending(x => x.Fecha).Paginate(paginacion).ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<IActionResult> GetPaginas([FromQuery] PaginationDTO paginacion)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            if (usuario is null) return BadRequest("Usuaio no válido");

            var queryable = _context.Ordenes.AsQueryable();

            var esAdministrador = await _userHelper.IsUserInRoleAsync(usuario, UserType.Admin.ToString());

            if (!esAdministrador) queryable = queryable.Where(x => x.Usuario!.Email == User.Identity!.Name);

            double cantidad = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(cantidad / paginacion.RecordsNumber);

            return Ok(totalPaginas);
        }

        [HttpGet("{ordenId:int}")]
        public async Task<IActionResult>GetAsync(int OrdenId)
        {
            var orden = await _context.Ordenes.Include(o => o.Usuario).ThenInclude(u => u.Municipio).ThenInclude(m => m.Estado).ThenInclude(e => e.Pais)
                .Include(o => o.OrdenDetalles!).ThenInclude(od => od.Producto).ThenInclude(p => p.ProductoImagenes).FirstOrDefaultAsync(o => o.Id == OrdenId);

            if(orden is null) return NotFound();

            return Ok(orden);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(OrdenDTO ordenDTO)
        {
            var response = await _ordenHelper.ProcesarOrdenAsync(User.Identity!.Name!, ordenDTO.Comentarios);

            if (response.IsSucces) return NoContent();

            return BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(OrdenDTO ordenDTO)
        {
            var usuario = await _userHelper.GetUserAsync(User.Identity!.Name!);

            if(usuario is null) return NotFound();

            var esAdministrador = await _userHelper.IsUserInRoleAsync(usuario, UserType.Admin.ToString());

            if (!esAdministrador && ordenDTO.OrdenEstatus != OrdenEstatus.Cancelado) return BadRequest("Necesitar ser administrador para ejecutar esta acción");

            var orden = await _context.Ordenes.Include(o => o.OrdenDetalles).FirstOrDefaultAsync(o => o.Id == ordenDTO.Id);

            if(orden is null) return NotFound();

            if (ordenDTO.OrdenEstatus == OrdenEstatus.Cancelado) await IncrementarExistenciaAsync(orden);

            orden.OrdenEstatus = ordenDTO.OrdenEstatus;
            _context.Ordenes.Update(orden);
            await _context.SaveChangesAsync();
            return Ok(orden);
        }

        private async Task IncrementarExistenciaAsync(Orden orden)
        {
            foreach (var ordenDetalle in orden.OrdenDetalles!)
            {
                var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == ordenDetalle.ProductoId);

                if (producto is not null) producto.Stock += ordenDetalle.Cantidad;
            }

            await _context.SaveChangesAsync();
        }
    }
}