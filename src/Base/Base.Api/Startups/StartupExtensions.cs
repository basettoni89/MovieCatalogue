using Base.Api.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using Base.Api.Swagger;
using Microsoft.IdentityModel.Tokens;
using Base.Api.Security;
using Autofac;

namespace Base.Api.Startups
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCustomMvc<TStartup>(this IServiceCollection services) where TStartup : IStartup
        {
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TStartup>())
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddApiVersioning();
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }

        public static IServiceCollection AddCustomSwagger<TStartup>(this IServiceCollection services) where TStartup : IStartup
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions<TStartup>>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header

                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                });

            return services;
        }

        public static IServiceCollection AddCustomAuthentication<TStartup>(this IServiceCollection services, IConfiguration configuration, TStartup startup) where TStartup : IStartup
        {
            services.Configure<TokenSettings>(configuration.GetSection(TokenSettings.SettingsKey));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    var tokenService = startup.AutofacContainer.Resolve<ITokenService>();
                    var tokenConfig = configuration.GetSection(TokenSettings.SettingsKey).Get<TokenSettings>();
                    var key = tokenService.ReadConfigPublicKey();

                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = tokenConfig.Issuer,
                        ValidAudience = tokenConfig.Audience,
                        IssuerSigningKey = key
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
