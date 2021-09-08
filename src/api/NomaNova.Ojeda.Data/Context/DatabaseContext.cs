using System;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Core.Helpers.Interfaces;
using NomaNova.Ojeda.Data.Context.Interfaces;
using NomaNova.Ojeda.Data.Seeders;

namespace NomaNova.Ojeda.Data.Context
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Field> Fields { get; set; }

        public DbSet<FieldSet> FieldSets { get; set; }
        
        public DbSet<FieldSetField> FieldSetFields { get; set; }

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
            FieldsSeeder.Seed(this, _timeKeeper).Wait();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            BuildForFields(builder);
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
    }
}