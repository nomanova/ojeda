using System;
using System.Linq;
using Hangfire;
using Hangfire.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NomaNova.Ojeda.Api.Middleware;
using NomaNova.Ojeda.Api.Options;
using NomaNova.Ojeda.Api.Options.Application;
using NomaNova.Ojeda.Api.Options.Framework;
using NomaNova.Ojeda.Api.Utils;
using NomaNova.Ojeda.Core.Domain.AssetAttachments;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Context;
using NomaNova.Ojeda.Data.Context.Interfaces;
using NomaNova.Ojeda.Data.Options;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.Features.AssetAttachments;
using NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;
using NomaNova.Ojeda.Services.Features.AssetIds;
using NomaNova.Ojeda.Services.Features.AssetIds.Interfaces;
using NomaNova.Ojeda.Services.Features.AssetIdTypes;
using NomaNova.Ojeda.Services.Features.AssetIdTypes.Interfaces;
using NomaNova.Ojeda.Services.Features.Assets;
using NomaNova.Ojeda.Services.Features.Assets.Interfaces;
using NomaNova.Ojeda.Services.Features.AssetTypes;
using NomaNova.Ojeda.Services.Features.AssetTypes.Interfaces;
using NomaNova.Ojeda.Services.Features.Fields;
using NomaNova.Ojeda.Services.Features.Fields.Interfaces;
using NomaNova.Ojeda.Services.Features.FieldSets;
using NomaNova.Ojeda.Services.Features.FieldSets.Interfaces;
using NomaNova.Ojeda.Services.Shared.Background;
using NomaNova.Ojeda.Services.Shared.Background.Interfaces;
using NomaNova.Ojeda.Services.Shared.FileStore;
using NomaNova.Ojeda.Services.Shared.FileStore.Interfaces;
using NomaNova.Ojeda.Utils.Services;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Api;

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
        AddHangfire(services);
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

        if (!EnvUtils.IsTesting())
        {
            databaseContext.EnsureMigrated();
        }

        if (_appOptions.Database.RunSeeders)
        {
            databaseContext.EnsureSeeded();
        }

        UseSwagger(app);

        app.UseRouting();
        app.UseCors(Constants.CorsPolicy);

        app.UseRequestResponseLogging();
        app.UseExceptionMiddleware();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        UseHangfire(app);
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
        services.TryAddSingleton<IJobService, JobService>();

        services.TryAddSingleton<ISymbologyService, SymbologyService>();
        services.TryAddSingleton<IEan13SymbologyService, Ean13SymbologyService>();

        services.TryAddSingleton<IFieldDataConverter, FieldDataConverter>();

        services.TryAddScoped<IFieldsService, FieldsService>();
        services.TryAddScoped<IFieldSetsService, FieldSetsService>();
        services.TryAddScoped<IAssetTypesService, AssetTypesService>();
        services.TryAddScoped<IAssetsService, AssetsService>();
        services.TryAddScoped<IAssetIdTypesService, AssetIdTypesService>();
        services.TryAddScoped<IAssetIdsService, AssetIdsService>();
        services.TryAddScoped<IAssetAttachmentsService, AssetAttachmentsService>();
        
        services.TryAddScoped<IThumbnailGenerator, ThumbnailGenerator>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.TryAddScoped<IRepository<Field>, EntityRepository<Field>>();
        services.TryAddScoped<IRepository<FieldSet>, EntityRepository<FieldSet>>();
        services.TryAddScoped<IRepository<AssetType>, EntityRepository<AssetType>>();
        services.TryAddScoped<IRepository<Asset>, EntityRepository<Asset>>();
        services.TryAddScoped<IRepository<FieldValue>, EntityRepository<FieldValue>>();
        services.TryAddScoped<IRepository<AssetIdType>, EntityRepository<AssetIdType>>();
        services.TryAddScoped<IRepository<AssetAttachment>, EntityRepository<AssetAttachment>>();
    }

    private void AddFileStore(IServiceCollection services)
    {
        switch (_appOptions.FileStore.Type)
        {
            case FileStoreType.FileSystem:
                services.TryAddSingleton<IFileStore, FileSystem>();
                const string fileSystemSectionName =
                    $"{nameof(AppOptions.FileStore)}:{nameof(AppOptions.FileStore.FileSystem)}";
                services.ConfigureAndValidate<FileSystemOptions>(fileSystemSectionName, _configuration);
                break;
            default:
                throw new NotImplementedException(_appOptions.FileStore.Type.ToString());
        }
        
        services.TryAddSingleton<IAssetFileStore, AssetFileStore>();
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

    private void AddHangfire(IServiceCollection services)
    {
        if (EnvUtils.IsTesting())
        {
            return;
        }

        services.AddHangfire(config =>
        {
            HangfireDatabaseContextBuilder.Build(_appOptions.Database, config);
            config.UseConsole();
        });

        services.AddHangfireServer();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(Swagger.GetDoc(), Swagger.GetInfo());
            options.EnableAnnotations();
            options.IncludeXmlComments(Swagger.GetXmlPath(typeof(Startup).Assembly));
            options.UseOneOfForPolymorphism();
            options.SelectSubTypesUsing(baseType =>
            {
                return typeof(ErrorDto).Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType));
            });
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
        app.UseSwaggerUI(c => { c.SwaggerEndpoint(Swagger.GetJsonPath(), Swagger.GetDefinition()); });
    }

    private static void UseHangfire(IApplicationBuilder app)
    {
        if (EnvUtils.IsTesting() || !EnvUtils.IsDevelopment())
        {
            return;
        }

        app.UseHangfireDashboard();
    }
}