namespace NomaNova.Ojeda.Api.Services.Interfaces
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string obj);
    }
}