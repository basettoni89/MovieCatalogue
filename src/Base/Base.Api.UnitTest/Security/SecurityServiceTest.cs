using Base.Api.Security;
using Base.Api.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Base.Api.UnitTest.Security
{
    public class SecurityServiceTest
    {

        [Fact]
        public void ReadJWKLocalPrivateKeyTest()
        {
            var tokenSettings = new OptionsWrapper<TokenSettings>(new TokenSettings
            {
                PrivateKeyPath = Path.Combine("Keys", "debug.jwtJWK.key")
            }); ;

            TokenService service = new TokenService(tokenSettings);

            var key = service.ReadConfigPrivateKey();

            Assert.NotNull(key);
        }

        [Fact]
        public void ReadJWKLocalPublicKeyTest()
        {
            var tokenSettings = new OptionsWrapper<TokenSettings>(new TokenSettings
            {
                PublicKeyPath = Path.Combine("Keys", "debug.jwtJWK.key.pub")
            }); ;

            TokenService service = new TokenService(tokenSettings);

            var key = service.ReadConfigPublicKey();

            Assert.NotNull(key);
        }
    }
}
