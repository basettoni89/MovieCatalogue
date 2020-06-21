using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Base.Api.Security
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor context;

        public IdentityService(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public string GetUserIdentity()
        {
            return context.HttpContext.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        }

        public Guid? GetSessionId()
        {
            Guid sessionId;

            if (Guid.TryParse(context.HttpContext.User?.FindFirst(JwtRegisteredClaimNames.Sid)?.Value, out sessionId))
                return sessionId;
            else
                return null;
        }
    }
}
