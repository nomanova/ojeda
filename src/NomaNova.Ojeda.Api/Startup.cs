using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NomaNova.Ojeda.Api.Database;
using NomaNova.Ojeda.Api.Database.Interfaces;
using NomaNova.Ojeda.Api.Options;
using NomaNova.Ojeda.Api.Options.Application;
using NomaNova.Ojeda.Api.Options.Framework;
using NomaNova.Ojeda.Api.Services;
using NomaNova.Ojeda.Api.Services.Interfaces;
using NomaNova.Ojeda.Api.Utils;

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
            AddDatabase(services);
            AddAutoMapper(services);
            AddSwagger(services);
            AddMvc(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();
            UseSwagger(app);
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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

        private static void AddDatabase(IServiceCollection services)
        {
            services.TryAddSingleton<IDatabaseContextBuilder, DatabaseContextBuilder>();

            services.AddDbContext<DatabaseContext>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Swagger.GetDoc(), Swagger.GetInfo());
                options.IncludeXmlComments(Swagger.GetXmlPath(typeof(Startup).Assembly));
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        private static void AddMvc(IServiceCollection services)
        {
            services
                .AddMvc(AppMvcOptions.Apply)
                .ConfigureApiBehaviorOptions(AppApiBehaviorOptions.Apply)
                .AddNewtonsoftJson(Serializer.ApplyJsonSettings);
        }
        
        private static void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Swagger.GetJsonPath(), Swagger.GetDefinition());
            });
        }
    }
}