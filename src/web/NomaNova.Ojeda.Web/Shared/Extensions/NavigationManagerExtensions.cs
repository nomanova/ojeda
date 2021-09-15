using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace NomaNova.Ojeda.Web.Shared.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(string))
                {
                    
                    value = (T) (object) valueFromQueryString[0];
                    return true;
                }

                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T) (object) valueAsInt;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}