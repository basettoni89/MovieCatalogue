using System;

namespace Base.Api.Security
{
    public interface IIdentityService
    {
        Guid? GetSessionId();
        string GetUserIdentity();
    }
}