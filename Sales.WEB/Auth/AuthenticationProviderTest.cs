using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Sales.WEB.Auth
{
    public class AuthenticationProviderTest: AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var pruebaUser = new ClaimsIdentity(new List<Claim> {
                new Claim("FirstName", "Valentin"),
                new Claim("LastName", "Villegas"),
                new Claim(ClaimTypes.Name, "valentinvillegas22@yopmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            }, authenticationType: "test") ;
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(pruebaUser)));
        }
    }
}