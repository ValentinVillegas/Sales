using Microsoft.AspNetCore.Identity;
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
    }
}
