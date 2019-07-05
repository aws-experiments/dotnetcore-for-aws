using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnetcore.For.Aws.Domain.Config;
using Dotnetcore.For.Aws.Service.Infra;
using dotnetcore_for_aws.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dotnetcore.For.Aws.Serviceopenid
{
    public class Startup
    {
        private ConfigSettingModel _configSettingModel;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _configSettingModel = services.RegisterConfig(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.RegisterIoc(Configuration);
            services.RegisterOAuth(_configSettingModel);

            services.RegisterSwagger(_configSettingModel);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/V1/swagger.json", "aws-experiments dotnetcore-for-aws API");
                options.OAuthClientId(_configSettingModel.Auth.CognitoClientId);
                options.OAuthAppName(_configSettingModel.Auth.AppName);
            });
            app.UseMvc();
        }
    }
}
