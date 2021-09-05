using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NomaNova.Ojeda.Core.Helpers.Interfaces;

namespace NomaNova.Ojeda.Api.Tests.Controllers.Base
{
    public class ApiTests
    {
        private const string TestEnvironment = Constants.EnvTest;
        private const string ApiServerUrl = "http://localhost:60000";

        protected readonly TestServer ApiServer;
        protected readonly HttpClient ApiClient;

        protected ApiTests()
        {
            Environment.SetEnvironmentVariable(Constants.AspNetCoreEnvironmentVar, TestEnvironment);

            var sutPath = ToAbsolutePath("../../../../../../src/api/NomaNova.Ojeda.Api");
            var testPath = ToAbsolutePath("../../../../../../test/api/NomaNova.Ojeda.Api.Tests");

            var apiBuilder = new WebHostBuilder()
                .UseUrls(ApiServerUrl)
                .UseEnvironment(TestEnvironment)
                .UseContentRoot(sutPath)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile(
                        $"{testPath}/appsettings.json", false, false
                    );
                })
                .UseStartup<Startup>()
                .ConfigureTestServices(RegisterServices);

            ApiServer = new TestServer(apiBuilder);
            ApiClient = ApiServer.CreateClient();
        }

        protected virtual void RegisterServices(IServiceCollection services)
        {
            // NOP - override in subclass(es) as needed
        }

        protected TService GetService<TService>()
        {
            return (TService) ApiServer.Host.Services.GetService(typeof(TService));
        }

        protected async Task<T> GetPayloadAsync<T>(HttpResponseMessage response)
        {
            var serializer = GetService<ISerializer>();
            var content = await response.Content.ReadAsStringAsync();

            return serializer.Deserialize<T>(content);
        }

        private static string ToAbsolutePath(string relativePath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var absolutePath = Path.GetFullPath(Path.Combine(currentDirectory, relativePath));
            if (!Directory.Exists(absolutePath))
            {
                throw new IOException($"Cannot find path: {relativePath}");
            }

            return absolutePath;
        }
    }
}