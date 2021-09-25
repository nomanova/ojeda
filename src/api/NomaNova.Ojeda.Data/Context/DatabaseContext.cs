using System;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
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
        private readonly IDatabaseContextBuilder _databaseContextBuilder;

        public DatabaseContext(
            ITimeKeeper timeKeeper,
            IDatabaseContextBuilder databaseContextBuilder,
            DbContextOptions<DatabaseContext> contextOptions)
            : base(contextOptions)
        {
            _timeKeeper = timeKeeper;
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
            
            BuildForFields(builder);
            BuildForFieldSets(builder);
            BuildForAssetTypes(builder);
            BuildForAssets(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _databaseContextBuilder.Build(optionsBuilder);
        }

        private static void BuildForFields(ModelBuilder builder)
        {
            // Field
            builder.Entity<Field>()
                .HasKey(_ => _.Id);
            
            builder.Entity<Field>()
                .Property(_ => _.Type)
                .HasConversion(fieldType => fieldType.ToString(), s => Enum.Parse<FieldType>(s));
        }

        private static void BuildForFieldSets(ModelBuilder builder)
        {
            // FieldSet
            builder.Entity<FieldSet>()
                .HasKey(_ => _.Id);
            
            // FieldSetField
            builder.Entity<FieldSetField>()
                .HasKey(_ => new {_.FieldId, _.FieldSetId});

            builder.Entity<FieldSetField>()
                .HasOne(_ => _.Field)
                .WithMany(_ => _.FieldSetFields)
                .HasForeignKey(_ => _.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FieldSetField>()
                .HasOne(_ => _.FieldSet)
                .WithMany(_ => _.FieldSetFields)
                .HasForeignKey(_ => _.FieldSetId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void BuildForAssetTypes(ModelBuilder builder)
        {
            // AssetType
            builder.Entity<AssetType>()
                .HasKey(_ => _.Id);
            
            // AssetTypeFieldSet
            builder.Entity<AssetTypeFieldSet>()
                .HasKey(_ => new {_.FieldSetId, _.AssetTypeId});
            
            builder.Entity<AssetTypeFieldSet>()
                .HasOne(_ => _.FieldSet)
                .WithMany(_ => _.AssetTypeFieldSets)
                .HasForeignKey(_ => _.FieldSetId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<AssetTypeFieldSet>()
                .HasOne(_ => _.AssetType)
                .WithMany(_ => _.AssetTypeFieldSets)
                .HasForeignKey(_ => _.AssetTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void BuildForAssets(ModelBuilder builder)
        {
            // Asset
            builder.Entity<Asset>()
                .HasKey(_ => _.Id);
        
            builder.Entity<Asset>()
                .HasOne(_ => _.AssetType)
                .WithMany(_ => _.Assets)
                .HasForeignKey(_ => _.AssetTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // FieldValue
            builder.Entity<FieldValue>()
                .HasKey(_ => _.Id);
        
            builder.Entity<FieldValue>()
                .HasOne(_ => _.Asset)
                .WithMany(_ => _.FieldValues)
                .HasForeignKey(_ => _.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        
            builder.Entity<FieldValue>()
                .HasOne(_ => _.FieldSet);
            
            builder.Entity<FieldValue>()
                .HasOne(_ => _.Field);
        }
    }
}