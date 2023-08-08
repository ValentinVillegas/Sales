using Microsoft.AspNetCore.Identity;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Helpers
{
    public interface IUserHelper
    {
        Task<Usuario> GetUserAsync(string email);
        Task<IdentityResult> AddUserAsync(Usuario usuario, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(Usuario usuario, string roleName);
        Task<bool> IsUserInRoleAsync(Usuario usuario, string reoleName);
        Task<SignInResult> LoginAsync(LoginDTO model);
        Task LogoutAsync();
        Task<IdentityResult> ChangePasswordAsync(Usuario user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateUserAsync(Usuario user);
        Task<Usuario> GetUserAsync(Guid userId);
        Task<string> GenerateEmailConfirmationTokenAsync(Usuario user);
        Task<IdentityResult> ConfirmEmailAsync(Usuario user, string token);
    }
}
