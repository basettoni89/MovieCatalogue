using Base.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace Base.Api.Security
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings settings;

        public TokenService(IOptions<TokenSettings> settings)
        {
            this.settings = settings.Value;
        }

        public SecurityKey ReadConfigPublicKey()
        {
            return ReadKey(settings.PublicKeyPath);
        }

        public SecurityKey ReadConfigPrivateKey()
        {
            return ReadKey(settings.PrivateKeyPath);
        }

        private SecurityKey ReadKey(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return JsonWebKey.Create(reader.ReadToEnd());
            }
        }
    }
}
