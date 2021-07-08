using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NomaNova.Ojeda.Api.Options;
using NomaNova.Ojeda.Api.Options.Application;
using MsOptions = Microsoft.Extensions.Options;

namespace NomaNova.Ojeda.Api.Database.Context
{
    public abstract class ContextFactory
    {
        private static MsOptions.IOptions<DatabaseOptions> GetDatabaseOptions()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile(Constants.AppSettingsFile, optional: false)
                .AddEnvironmentVariables()
                .Build();

            var configurationSection = configuration.GetSection(nameof(AppOptions.Database));

            var connectionString = configurationSection.GetValue<string>(nameof(AppOptions.Database.ConnectionString));
            var type = configurationSection.GetValue<DatabaseType>(nameof(AppOptions.Database.Type));
            
            var databaseOptions = MsOptions.Options.Create(new DatabaseOptions
            {
                Type = type,
                ConnectionString = connectionString
            });

            return databaseOptions;
        }
        
        protected static DatabaseContextBuilder CreateContextBuilder()
        {
            return new DatabaseContextBuilder(GetDatabaseOptions());
        }
    }

    public class DatabaseContextFactory : ContextFactory, IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var contextBuilder = CreateContextBuilder();
            var contextOptions = new DbContextOptionsBuilder<DatabaseContext>();
            
            return new DatabaseContext(contextBuilder, contextOptions.Options);
        }
    }
}