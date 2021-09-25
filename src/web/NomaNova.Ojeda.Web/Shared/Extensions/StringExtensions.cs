namespace NomaNova.Ojeda.Web.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxChars = Constants.DefaultTruncateSmall)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + (char)0x2026;
        }
    }
}