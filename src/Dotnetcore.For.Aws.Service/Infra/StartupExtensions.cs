using Dotnetcore.For.Aws.Domain.Config;
using dotnetcore_for_aws.Infra.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnetcore.For.Aws.Service.Infra
{
    public static class StartupExtensions
    {
        private const string AuthSpec = "oauth2";
        private const string AuthMode = "implicit";
        public static string SectionName = "Config";

        public static ConfigSettingModel RegisterConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigSettingModel>(configuration.GetSection(SectionName)).
                AddSingleton(x => x.GetRequiredService<IOptions<ConfigSettingModel>>().Value);
            return services.BuildServiceProvider().GetService<ConfigSettingModel>();
        }

        public static void RegisterSwagger(this IServiceCollection services, ConfigSettingModel configSetting)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(configSetting.Swagger.Doc.Version, new Info
                {
                    Title = configSetting.Swagger.Doc.Title,
                    Version = configSetting.Swagger.Doc.Version,
                    Description = configSetting.Swagger.Doc.Desc
                });
                c.AddSecurityDefinition(AuthSpec, new OAuth2Scheme
                {
                    
                    Flow = AuthMode,
                    AuthorizationUrl = $"{configSetting.Auth.BaseUrl}/oauth2/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        { "email","" },
                        { "openid",""},
                        { "http://localhost:8080/api.aws-exp.v1",""},
                        { "profile",""}
                    }
                });
                c.OperationFilter<AuthorizationHeaderOperationFilter>();
            });
        }

        public static void RegisterOAuth(this IServiceCollection services, ConfigSettingModel configSetting)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configSetting.Auth.Authority;
                    options.Audience = configSetting.Auth.CognitoClientId;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
        }
    }
}
