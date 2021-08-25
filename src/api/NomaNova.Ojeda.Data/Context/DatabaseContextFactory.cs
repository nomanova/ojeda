using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NomaNova.Ojeda.Core.Helpers;
using NomaNova.Ojeda.Data.Options;
using MsOptions = Microsoft.Extensions.Options;

namespace NomaNova.Ojeda.Data.Context
{
    public abstract class ContextFactory
    {
        private const string DatabaseTypeEnvVar = "OJEDA_DATABASE_TYPE";
        private const string DatabaseConnectionStringEnvVar = "OJEDA_DATABASE_CONNECTION_STRING";
        
        private static MsOptions.IOptions<DatabaseOptions> GetDatabaseOptions()
        {
            var envDatabaseType = Environment.GetEnvironmentVariable(DatabaseTypeEnvVar);

            if (string.IsNullOrEmpty(envDatabaseType) ||
                !Enum.TryParse(envDatabaseType, true, out DatabaseType databaseType))
            {
                throw new ArgumentException("Missing or unknown database type");
            }

            var databaseConnectionString = Environment.GetEnvironmentVariable(DatabaseConnectionStringEnvVar);

            if (string.IsNullOrEmpty(databaseConnectionString))
            {
                throw new ArgumentException("Missing database connection string");
            }
            
            var databaseOptions = MsOptions.Options.Create(new DatabaseOptions
            {
                Type = databaseType,
                ConnectionString = databaseConnectionString
            });

            return databaseOptions;
        }
        
        protected static DatabaseContextBuilder CreateContextBuilder()
        {
            return new(GetDatabaseOptions());
        }
    }

    public class DatabaseContextFactory : ContextFactory, IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var contextBuilder = CreateContextBuilder();
            var timeKeeper = new TimeKeeper();
            var contextOptions = new DbContextOptionsBuilder<DatabaseContext>();
            
            return new DatabaseContext(timeKeeper, contextBuilder, contextOptions.Options);
        }
    }
}