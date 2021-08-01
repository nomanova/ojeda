using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NomaNova.Ojeda.Data.Context.Interfaces;
using NomaNova.Ojeda.Data.Options;

namespace NomaNova.Ojeda.Data.Context
{
    public class DatabaseContextBuilder : IDatabaseContextBuilder
    {
        private const string MigrationsHistoryTable = "Migrations";
        
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
            optionsBuilder.UseSqlServer(_options.ConnectionString,
                x => x.MigrationsHistoryTable(MigrationsHistoryTable));   
        }
        
        private void BuildPostgresql(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.ConnectionString, 
                x => x.MigrationsHistoryTable(MigrationsHistoryTable));
        }

        private void BuildSqlite(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_options.ConnectionString,
                x => x.MigrationsHistoryTable(MigrationsHistoryTable));
        }
    }
}