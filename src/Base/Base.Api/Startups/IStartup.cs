using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Api.Startups
{
    public interface IStartup
    {
        ILifetimeScope AutofacContainer { get; }
    }
}
