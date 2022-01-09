using System;
using Hangfire;
using Hangfire.PostgreSql;
using NomaNova.Ojeda.Data.Options;

namespace NomaNova.Ojeda.Data.Context;

public static class HangfireDatabaseContextBuilder
{
    private const string DefaultSchema = "hangfire";

    public static void Build(DatabaseOptions options, IGlobalConfiguration configuration)
    {
        switch (options.Type)
        {
            case DatabaseType.Postgresql:
                BuildPostgresql(options.ConnectionString, configuration);
                break;
            default:
                throw new NotImplementedException(options.Type.ToString());
        }
    }

    private static void BuildPostgresql(string connectionString, IGlobalConfiguration configuration)
    {
        configuration.UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
        {
            SchemaName = DefaultSchema
        });
    }
}