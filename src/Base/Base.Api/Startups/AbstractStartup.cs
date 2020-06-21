using Autofac;
using Autofac.Extensions.DependencyInjection;
using Base.Api.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Api.Startups
{
    public abstract class AbstractStartup : IStartup
    {

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public AbstractStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract void ConfigureServices(IServiceCollection services);

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<BaseApiModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                    }
                });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (Configuration.GetValue<bool>("BypassSecurity"))
                {
                    app.UseMiddleware<BypassAuthenticationMiddleware>();
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
