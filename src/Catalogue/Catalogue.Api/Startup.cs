using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Api.Startups;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalogue.Api
{
    public class Startup : AbstractStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc<Startup>()
                .AddCustomAuthentication<Startup>(Configuration, this)
                .AddCustomSwagger<Startup>()
                .AddCustomIntegrations(Configuration);
        }
    }
}
