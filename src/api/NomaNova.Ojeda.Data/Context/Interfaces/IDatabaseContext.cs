namespace NomaNova.Ojeda.Data.Context.Interfaces
{
    public interface IDatabaseContext
    {
        void EnsureMigrated();
        
        void EnsureSeeded();
    }
}