using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NomaNova.Ojeda.Api.FileStore;
using NomaNova.Ojeda.Api.FileStore.Interfaces;
using NomaNova.Ojeda.Api.Middleware;
using NomaNova.Ojeda.Api.Options;
using NomaNova.Ojeda.Api.Options.Application;
using NomaNova.Ojeda.Api.Options.Framework;
using NomaNova.Ojeda.Api.Utils;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Core.Helpers;
using NomaNova.Ojeda.Core.Helpers.Interfaces;
using NomaNova.Ojeda.Data.Context;
using NomaNova.Ojeda.Data.Context.Interfaces;
using NomaNova.Ojeda.Data.Options;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Services.AssetClasses;
using NomaNova.Ojeda.Services.Assets;
using NomaNova.Ojeda.Services.Fields;
using NomaNova.Ojeda.Services.FieldSets;

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
            AddRepositories(services);
            AddFileStore(services);
            AddDatabase(services);
            AddAutoMapper(services);
            AddCors(services);
            AddSwagger(services);
            AddMvc(services);
        }

        public void Configure(
            IApplicationBuilder app,
            DatabaseContext databaseContext)
        {
            app.UseFileServer();

            if (_appOptions.Database.RunSeeders)
            {
                databaseContext.EnsureSeeded();
            }

            UseSwagger(app);
            
            app.UseRouting();
            app.UseCors(Constants.CorsPolicy);

            app.UseExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            
            services.ConfigureAndValidate<GlobalOptions>(nameof(AppOptions.Global), _configuration);
            services.ConfigureAndValidate<FileStoreOptions>(nameof(AppOptions.FileStore), _configuration);
            services.ConfigureAndValidate<DatabaseOptions>(nameof(AppOptions.Database), _configuration);
            services.ConfigureAndValidate<SecurityOptions>(nameof(AppOptions.Security), _configuration);
            
            _appOptions.Global = _configuration.GetSettings<GlobalOptions>(nameof(AppOptions.Global));
            _appOptions.FileStore = _configuration.GetSettings<FileStoreOptions>(nameof(AppOptions.FileStore));
            _appOptions.Database = _configuration.GetSettings<DatabaseOptions>(nameof(AppOptions.Database));
            _appOptions.Security = _configuration.GetSettings<SecurityOptions>(nameof(AppOptions.Security));
        }

        private static void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<ITimeKeeper, TimeKeeper>();
            services.TryAddSingleton<ISerializer, Serializer>();
            
            services.TryAddScoped<IFieldsService, FieldsService>();
            services.TryAddScoped<IFieldSetsService, FieldSetsService>();
            services.TryAddScoped<IAssetClassesService, AssetClassesService>();
            services.TryAddScoped<IAssetsService, AssetsService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.TryAddScoped<IRepository<Field>, EntityRepository<Field>>();
            services.TryAddScoped<IRepository<FieldSet>, EntityRepository<FieldSet>>();
            services.TryAddScoped<IRepository<AssetClass>, EntityRepository<AssetClass>>();
            services.TryAddScoped<IRepository<Asset>, EntityRepository<Asset>>();
        }

        private void AddFileStore(IServiceCollection services)
        {
            switch (_appOptions.FileStore.Type)
            {
                case FileStoreType.FileSystem:
                    services.TryAddSingleton<IFileStore, FileSystem>();
                    services.ConfigureAndValidate<FileSystemOptions>(nameof(AppOptions.FileStore.FileSystem), _configuration);
                    break;
                default:
                    throw new NotImplementedException(_appOptions.FileStore.Type.ToString());
            }
        }
        
        private static void AddDatabase(IServiceCollection services)
        {
            services.TryAddSingleton<IDatabaseContextBuilder, DatabaseContextBuilder>();
            services.AddDbContext<DatabaseContext>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FieldProfile).Assembly);
        }

        private void AddCors(IServiceCollection services)
        {
            services.AddCors(options => AppCorsOptions.Apply(options, _appOptions.Security));
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Swagger.GetDoc(), Swagger.GetInfo());
                options.EnableAnnotations();
                options.IncludeXmlComments(Swagger.GetXmlPath(typeof(Startup).Assembly));
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        private static void AddMvc(IServiceCollection services)
        {
            services
                .AddMvc(AppMvcOptions.Apply)
                .ConfigureApiBehaviorOptions(AppApiBehaviorOptions.Apply)
                .AddNewtonsoftJson(AppMvcOptions.Apply);
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