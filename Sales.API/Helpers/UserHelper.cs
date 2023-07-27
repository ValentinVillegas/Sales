using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entidades;

namespace Sales.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(DataContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(Usuario usuario, string password)
        {
            return await _userManager.CreateAsync(usuario, password);
        }

        public async Task AddUserToRoleAsync(Usuario usuario, string roleName)
        {
            await _userManager.AddToRoleAsync(usuario, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole {Name = roleName});
            }
        }

        public async Task<Usuario> GetUserAsync(string email)
        {
            return await _context.Users.Include(u => u.Municipio).ThenInclude(m => m.Estado).ThenInclude(e => e.Pais).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> IsUserInRoleAsync(Usuario usuario, string reoleName)
        {
            return await _userManager.IsInRoleAsync(usuario, reoleName);
        }
    }
}
