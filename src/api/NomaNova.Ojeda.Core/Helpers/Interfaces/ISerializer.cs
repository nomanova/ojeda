namespace NomaNova.Ojeda.Core.Helpers.Interfaces
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string obj);
    }
}