using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Base.Api.Security
{
    public class BypassAuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public BypassAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentUserId = string.Empty;

            var authHeader = context.Request.Headers["Authorization"];
            if (authHeader != StringValues.Empty)
            {
                var header = authHeader.FirstOrDefault();
                if (!string.IsNullOrEmpty(header) && header.StartsWith("Username ") && header.Length > "Username ".Length)
                {
                    currentUserId = header.Substring("Username ".Length);
                }
            }


            if (!string.IsNullOrEmpty(currentUserId))
            {
                var user = new ClaimsIdentity(new[] {
                    new Claim("nonce", Guid.NewGuid().ToString()),
                    new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "BypassAuthenticationMiddleware"),
                    new Claim("nonce", Guid.NewGuid().ToString()),
                    new Claim("sub", currentUserId)}
                , "BypassAuthentication");

                context.User = new ClaimsPrincipal(user);
            }

            await next.Invoke(context);
        }
    }
}
