using System;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Context.Configurations;
using NomaNova.Ojeda.Data.Context.Interfaces;
using NomaNova.Ojeda.Data.Seeders;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Context
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Field> Fields { get; set; }

        public DbSet<FieldSet> FieldSets { get; set; }
        
        public DbSet<FieldSetField> FieldSetFields { get; set; }

        public DbSet<AssetType> AssetTypes { get; set; }
        
        public DbSet<AssetTypeFieldSet> AssetTypeFieldSets { get; set; }

        public DbSet<Asset> Assets { get; set; }
        
        public DbSet<FieldValue> FieldValues { get; set; }

        private readonly ITimeKeeper _timeKeeper;
        private readonly ISerializer _serializer;
        private readonly IDatabaseContextBuilder _databaseContextBuilder;

        public DatabaseContext(
            ITimeKeeper timeKeeper,
            ISerializer serializer,
            IDatabaseContextBuilder databaseContextBuilder,
            DbContextOptions<DatabaseContext> contextOptions)
            : base(contextOptions)
        {
            _timeKeeper = timeKeeper;
            _serializer = serializer;
            _databaseContextBuilder = databaseContextBuilder;
        }

        public void EnsureSeeded()
        {
            // Migrate database
            Database.Migrate();
            
            if (!this.AllMigrationsApplied())
            {
                throw new Exception("Could not apply all database migrations.");
            }

            // Run seeders
            FieldsSeeder.Seed(this, _timeKeeper).Wait();
            FieldSetsSeeder.Seed(this, _timeKeeper).Wait();
            AssetTypesSeeder.Seed(this, _timeKeeper).Wait();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new FieldEntityTypeConfiguration(_serializer));
            builder.ApplyConfiguration(new FieldSetEntityTypeConfiguration());
            builder.ApplyConfiguration(new FieldSetFieldEntityTypeConfiguration());
            builder.ApplyConfiguration(new AssetTypeFieldSetEntityTypeConfiguration());
            builder.ApplyConfiguration(new AssetEntityTypeConfiguration());
            builder.ApplyConfiguration(new FieldValueEntityTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _databaseContextBuilder.Build(optionsBuilder);
        }
    }
}