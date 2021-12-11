using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NomaNova.Ojeda.Data.Context;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers.Base
{
    [Collection(nameof(ApiTests))]
    public class ApiTests : IDisposable
    {
        private const string TestEnvironment = Constants.EnvTest;
        private const string ApiServerUrl = "http://localhost:60000";

        private readonly TestServer _apiServer;
        private DbContext _dbContext;

        protected readonly HttpClient ApiClient;

        protected DbContext DatabaseContext => _dbContext ??= GetService<DatabaseContext>();

        protected ApiTests()
        {
            Environment.SetEnvironmentVariable(Constants.AspNetCoreEnvironmentVar, TestEnvironment);

            var sutPath = ToAbsolutePath("../../../../../../src/api/NomaNova.Ojeda.Api");
            var testPath = ToAbsolutePath("../../../../../../test/api/NomaNova.Ojeda.Api.Tests");

            var apiBuilder = new WebHostBuilder()
                .UseUrls(ApiServerUrl)
                .UseEnvironment(TestEnvironment)
                .UseContentRoot(sutPath)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile(
                        $"{testPath}/appsettings.json", false, false
                    );
                })
                .UseStartup<Startup>()
                .ConfigureTestServices(RegisterServices);

            _apiServer = new TestServer(apiBuilder);
            ApiClient = _apiServer.CreateClient();
        }

        public void Dispose()
        {
            GetService<DatabaseContext>().Database.EnsureDeleted();
        }
        
        protected virtual void RegisterServices(IServiceCollection services)
        {
            // NOP - override in subclass(es) as needed
        }

        protected TService GetService<TService>()
        {
            return (TService) _apiServer.Host.Services.GetService(typeof(TService));
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