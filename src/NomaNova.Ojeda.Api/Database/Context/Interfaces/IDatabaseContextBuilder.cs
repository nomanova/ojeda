using Microsoft.EntityFrameworkCore;

namespace NomaNova.Ojeda.Api.Database.Context.Interfaces
{
    public interface IDatabaseContextBuilder
    {
        void Build(DbContextOptionsBuilder optionsBuilder);
    }
}