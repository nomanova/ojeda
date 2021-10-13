using System.Collections.Generic;

namespace NomaNova.Ojeda.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static bool HasElements<T>(this ICollection<T> input)
        {
            return input != null && input.Count > 0;
        }
    }
}