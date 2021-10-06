using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NomaNova.Ojeda.Client;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Utils.Services;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp =>
                new OjedaClientBuilder("https://localhost:5001").Build()
            );
            
            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredModal();

            builder.Services.AddSingleton<ITimeKeeper, TimeKeeper>();
            
            // Validators
            builder.Services.AddTransient<IValidator<CreateFieldDto>, CreateFieldDtoFieldValidator>();
            builder.Services.AddTransient<IValidator<UpdateFieldDto>, UpdateFieldDtoFieldValidator>();
            builder.Services.AddTransient<IValidator<TextFieldDataDto>, TextFieldDataDtoFieldValidator>();
            builder.Services.AddTransient<IValidator<NumberFieldDataDto>, NumberFieldDataDtoFieldValidator>();
            
            await builder.Build().RunAsync();
        }
    }
}