using System;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Context.Interfaces;
using NomaNova.Ojeda.Data.Seeders;

namespace NomaNova.Ojeda.Data.Context
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Field> Fields { get; set; }

        private readonly IDatabaseContextBuilder _databaseContextBuilder;

        public DatabaseContext(
            IDatabaseContextBuilder databaseContextBuilder,
            DbContextOptions<DatabaseContext> contextOptions)
            : base(contextOptions)
        {
            _databaseContextBuilder = databaseContextBuilder;
        }

        public void EnsureSeeded()
        {
            FieldsSeeder.Seed(this);
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
                .Property(f => f.Type)
                .HasConversion(fieldType => fieldType.ToString(), s => Enum.Parse<FieldType>(s));
        }
    }
}