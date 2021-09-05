# NomaNova - Ojeda

## Database

Prerequisites:

```
$ dotnet tool install --global dotnet-ef
$ cd src/api/NomaNova.Ojeda.Data
$ export OJEDA_DATABASE_TYPE="Postgresql"
$ export OJEDA_DATABASE_CONNECTION_STRING="User ID=ojeda;Password=ojeda;Host=localhost;Port=5432;Database=ojeda;"
```

To apply the migrations to the database:

```
$ dotnet ef database update
```

Create a new migration:

```
$ dotnet ef migrations add <migration-name> --output-dir Migrations
```

To revert a migration on the database, use the name of the last known-good migration:

```
dotnet ef database update <GoodMigrationName> (or 0 for initial migration)
```

To remove the last added migration:

```
$ dotnet ef migrations remove
```