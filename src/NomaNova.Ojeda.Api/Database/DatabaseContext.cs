using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Database.Interfaces;

namespace NomaNova.Ojeda.Api.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        private readonly IDatabaseContextBuilder _databaseContextBuilder;

        public DatabaseContext(
            IDatabaseContextBuilder databaseContextBuilder,
            DbContextOptions<DatabaseContext> contextOptions)
            : base(contextOptions)
        {
            _databaseContextBuilder = databaseContextBuilder;
        }

        public void EnsureSeeded(IApplicationBuilder app)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _databaseContextBuilder.Build(optionsBuilder);
        }
    }
}