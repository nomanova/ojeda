# NomaNova - Ojeda

## Database Migrations

Prerequisites:

```
$ dotnet tool install --global dotnet-ef
$ cd src/NomaNova.Ojeda.Api
$ export ASPNETCORE_ENVIRONMENT=Development
```

Create a new migration:

```
$ dotnet ef migrations add <migration-name> --output-dir Database/Migrations
```