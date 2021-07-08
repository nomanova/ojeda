using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NomaNova.Ojeda.Api.Database.Context.Interfaces;
using NomaNova.Ojeda.Api.Options.Application;

namespace NomaNova.Ojeda.Api.Database.Context
{
    public class DatabaseContextBuilder : IDatabaseContextBuilder
    {
        private readonly DatabaseOptions _options;
        
        public DatabaseContextBuilder(IOptions<DatabaseOptions> databaseOptions)
        {
            _options = databaseOptions.Value;
        }

        public void Build(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_options.Type)
            {
                case DatabaseType.Memory:
                    BuildMemory(optionsBuilder);
                    break;
                case DatabaseType.Mssql:
                    BuildMssql(optionsBuilder);
                    break;
                case DatabaseType.Postgresql:
                    BuildPostgresql(optionsBuilder);
                    break;
                case DatabaseType.Sqlite:
                    BuildSqlite(optionsBuilder);
                    break;
                default:
                    throw new NotImplementedException(_options.Type.ToString());
            }
        }

        private void BuildMemory(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_options.ConnectionString);
        }

        private void BuildMssql(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_options.ConnectionString);   
        }
        
        private void BuildPostgresql(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.ConnectionString);
        }

        private void BuildSqlite(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_options.ConnectionString);
        }
    }
}