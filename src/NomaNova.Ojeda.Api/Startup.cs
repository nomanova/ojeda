using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NomaNova.Ojeda.Api.Options;
using NomaNova.Ojeda.Api.Options.Application;
using NomaNova.Ojeda.Api.Options.Framework;
using NomaNova.Ojeda.Api.Services;
using NomaNova.Ojeda.Api.Services.Interfaces;

namespace NomaNova.Ojeda.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly AppOptions _appOptions;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _appOptions = new AppOptions();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            AddOptions(services);
            AddServices(services);
            AddAutoMapper(services);
            AddMvc(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            
            services.ConfigureAndValidate<GlobalOptions>(nameof(AppOptions.Global), _configuration);
            
            _appOptions.Global = _configuration.GetSettings<GlobalOptions>(nameof(AppOptions.Global));
        }

        private static void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<ITimeKeeper, TimeKeeper>();
            services.TryAddSingleton<ISerializer, Serializer>();
        }
        
        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup).Assembly);
        }
        
        private static void AddMvc(IServiceCollection services)
        {
            services.AddMvc()
                .ConfigureApiBehaviorOptions(AppApiBehaviorOptions.Apply)
                .AddNewtonsoftJson(Serializer.ApplyJsonSettings);
        }
    }
}