using Demo.Project2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Principal;

namespace Demo.Project2.Helper
{
    public class SecurityHelper
    {
        public async void SignIn(HttpContext httpContext, User user, string scheme)
        {
            var claimsIdentity = new ClaimsIdentity(GetUserClaims(user), scheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(scheme, claimsPrincipal);
        }

        public async void SignOut(HttpContext httpContext, string scheme)
        {
            await httpContext.SignOutAsync(scheme);
        }

        private IEnumerable<Claim> GetUserClaims(User account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
            };
            foreach (var roleAccount in account.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleAccount.Role.Name));
            }
            return claims;
        }
    }
}
