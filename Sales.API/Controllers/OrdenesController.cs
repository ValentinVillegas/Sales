using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Helpers;
using Sales.Shared.DTOs;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/ordenes")]
    public class OrdenesController:ControllerBase
    {
        private readonly IOrdenHelper _ordenHelper;

        public OrdenesController(IOrdenHelper ordenHelper)
        {
            _ordenHelper = ordenHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrdenDTO ordenDTO)
        {
            var response = await _ordenHelper.ProcesarOrdenAsync(User.Identity!.Name!, ordenDTO.Comentarios);

            if (response.IsSucces) return NoContent();

            return BadRequest(response.Message);
        }
    }
}
