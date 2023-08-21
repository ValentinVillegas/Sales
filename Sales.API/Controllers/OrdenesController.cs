using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> PostAsync(OrdenDTO ordenDTO)
        {
            var response = await _ordenHelper.ProcesarOrdenAsync(User.Identity!.Name!, ordenDTO.Comentarios);

            if (response.IsSucces) return NoContent();

            return BadRequest(response.Message);
        }
    }
}
