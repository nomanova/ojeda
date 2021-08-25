using System.Threading.Tasks;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NomaNova.Ojeda.Client;

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

            await builder.Build().RunAsync();
        }
    }
}