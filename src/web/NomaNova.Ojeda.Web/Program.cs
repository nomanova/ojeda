using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NomaNova.Ojeda.Client;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Utils.Services;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using NomaNova.Ojeda.Web.Features.App.Assets.Services;
using NomaNova.Ojeda.Web.Features.App.Assets.Services.Interfaces;

namespace NomaNova.Ojeda.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var environment = builder.HostEnvironment.Environment;
            var apiEndpoint = "https://localhost:5001/";
            
            if (environment.Equals(
                Constants.EnvProduction, StringComparison.InvariantCultureIgnoreCase))
            {
                apiEndpoint = builder.HostEnvironment.BaseAddress;
            }

            Console.WriteLine($"Environment: {environment}");
            Console.WriteLine($"Api Endpoint: {apiEndpoint}");
            
            builder.Services.AddScoped(sp =>
                new OjedaClientBuilder(apiEndpoint).Build()
            );
            
            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredModal();

            RegisterServices(builder.Services);
            RegisterValidators(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ITimeKeeper, TimeKeeper>();
            
            services.AddSingleton<ISymbologyService, SymbologyService>();
            services.AddSingleton<IEan13SymbologyService, Ean13SymbologyService>();
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            // Field Sets
            services.AddTransient<IValidator<CreateFieldSetDto>, CreateFieldSetDtoFieldValidator>();
            services.AddTransient<IValidator<UpdateFieldSetDto>, UpdateFieldSetDtoFieldValidator>();

            // Fields
            services.AddTransient<IValidator<CreateFieldDto>, CreateFieldDtoFieldValidator>();
            services.AddTransient<IValidator<UpdateFieldDto>, UpdateFieldDtoFieldValidator>();
            services.AddTransient<IValidator<TextFieldPropertiesDto>, TextFieldPropertiesDtoFieldValidator>();
            services.AddTransient<IValidator<NumberFieldPropertiesDto>, NumberFieldPropertiesDtoFieldValidator>();
            
            // Asset Type Ids
            services.AddTransient<IValidator<CreateAssetIdTypeDto>, CreateAssetIdTypeDtoFieldValidator>();
            services.AddTransient<IValidator<UpdateAssetIdTypeDto>, UpdateAssetIdTypeDtoFieldValidator>();
            services.AddTransient<IValidator<Ean13SymbologyPropertiesDto>, Ean13SymbologyPropertiesDtoFieldValidator>();
        }
    }
}