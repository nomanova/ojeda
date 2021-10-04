using System.Collections.Generic;
using Newtonsoft.Json;

namespace NomaNova.Ojeda.Utils.Services.Interfaces
{
    public interface ISerializer
    {
        string Serialize(object obj, ICollection<JsonConverter> converters = null);

        T Deserialize<T>(string obj, ICollection<JsonConverter> converters = null);
    }
}