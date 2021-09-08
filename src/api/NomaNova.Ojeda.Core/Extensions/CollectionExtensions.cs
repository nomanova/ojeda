using System.Collections.Generic;

namespace NomaNova.Ojeda.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool HasElements<T>(this IList<T> input)
        {
            return input != null && input.Count > 0;
        }
    }
}