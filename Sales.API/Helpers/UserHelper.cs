using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UserHelper(DataContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
            var user = await _context.Users.Include(u => u.Municipio).ThenInclude(m => m!.Estado).ThenInclude(e => e!.Pais).FirstOrDefaultAsync(x => x.Email == email);
            return user! ;
        }

        public async Task<Usuario> GetUserAsync(Guid userId)
        {
            var user = await _context.Users.Include(u => u.Municipio).ThenInclude(m => m!.Estado).ThenInclude(e => e.Pais).FirstOrDefaultAsync(u => u.Id == userId.ToString());
            return user!;
        }

        public async Task<IdentityResult> ChangePasswordAsync(Usuario user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(Usuario user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(Usuario usuario, string reoleName)
        {
            return await _userManager.IsInRoleAsync(usuario, reoleName);
        }

        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(Usuario user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Usuario user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(Usuario user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(Usuario user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }
    }
}
