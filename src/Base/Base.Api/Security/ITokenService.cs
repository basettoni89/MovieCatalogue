using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Base.Api.Security
{
    public interface ITokenService
    {
        SecurityKey ReadConfigPrivateKey();
        SecurityKey ReadConfigPublicKey();
    }
}