using Microsoft.EntityFrameworkCore;

namespace NomaNova.Ojeda.Data.Context.Interfaces
{
    public interface IDatabaseContextBuilder
    {
        void Build(DbContextOptionsBuilder optionsBuilder);
    }
}