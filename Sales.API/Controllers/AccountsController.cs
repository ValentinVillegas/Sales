﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/accounts")]
    public class AccountsController:ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly string _contenedor;

        public AccountsController(IUserHelper userHerlper, IConfiguration configuration, IFileStorage fileStorage)
        {
            _userHelper = userHerlper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _contenedor = "users"; //Contenedor de AzureStorage
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _userHelper.GetUserAsync(User.Identity!.Name!));
        }

        [HttpPost("CrearUsuario")]
        public async Task<ActionResult> CrearUsuario([FromBody] UserDTO model)
        {
            Usuario user = model;

            if (!string.IsNullOrEmpty(model.Foto))
            {
                var fotoUsuario = Convert.FromBase64String(model.Foto);
                model.Foto = await _fileStorage.SaveFileAsync(fotoUsuario, ".jpg", _contenedor);
            }

            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userHelper.AddUserToRoleAsync(user, user.TipoUsuario.ToString());
                return Ok(BuildToken(user));
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _userHelper.LoginAsync(model);

            if (result.Succeeded)
            {
                var user = await _userHelper.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }

            return BadRequest("Correo electrónico incorrecto");
        }

        [HttpPost("CambiarPassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> CambiarPasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);

            if (user is null) return NotFound();

            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if(!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()!.Description);

            return NoContent();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PutAsync(Usuario user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Foto))
                {
                    var fotoUsuario = Convert.FromBase64String(user.Foto);
                    user.Foto = await _fileStorage.SaveFileAsync(fotoUsuario, ".jpg", _contenedor);
                }

                var currentUser = await _userHelper.GetUserAsync(user.Email!);

                if (currentUser is null) return NotFound();

                currentUser.Documento = user.Documento;
                currentUser.Nombre = user.Nombre;
                currentUser.Apellido = user.Apellido;
                currentUser.Direccion = user.Direccion;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Foto = !string.IsNullOrEmpty(user.Foto) && user.Foto != currentUser.Foto ? user.Foto : currentUser.Foto;
                currentUser.MunicipioId = user.MunicipioId;

                var result = await _userHelper.UpdateUserAsync(currentUser);

                if(result.Succeeded) return NoContent();

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private TokenDTO BuildToken(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Role, user.TipoUsuario.ToString()),
                new Claim("Document", user.Documento),
                new Claim("FirstName", user.Nombre),
                new Claim("LastName", user.Apellido),
                new Claim("Adress", user.Direccion),
                new Claim("Photo", user.Foto ?? string.Empty),
                new Claim("CityId", user.MunicipioId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);  
            var expiration = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims : claims,
                expires : expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }

    }
}
