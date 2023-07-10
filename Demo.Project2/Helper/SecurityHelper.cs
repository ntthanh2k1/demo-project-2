using Demo.Project2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Principal;

namespace Demo.Project2.Helper
{
    public class SecurityHelper
    {
        public async Task Login(HttpContext httpContext, User user, string scheme)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!)
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role!.Name!));
            }
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            };
            await httpContext.SignInAsync(scheme, claimsPrincipal, authProperties);
        }

        public async Task Logout(HttpContext httpContext, string scheme)
        {
            await httpContext.SignOutAsync(scheme);
        }
    }
}
