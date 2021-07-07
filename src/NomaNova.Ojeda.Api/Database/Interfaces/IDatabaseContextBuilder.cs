using Microsoft.EntityFrameworkCore;

namespace NomaNova.Ojeda.Api.Database.Interfaces
{
    public interface IDatabaseContextBuilder
    {
        void Build(DbContextOptionsBuilder optionsBuilder);
    }
}