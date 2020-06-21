using Autofac;
using Base.Api.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Api
{
    public class BaseApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TokenService>()
                .As<ITokenService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<IdentityService>()
                .As<IIdentityService>()
                .InstancePerLifetimeScope();
        }
    }
}
