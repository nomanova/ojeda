namespace NomaNova.Ojeda.Utils.Services.Interfaces
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string obj);
    }
}