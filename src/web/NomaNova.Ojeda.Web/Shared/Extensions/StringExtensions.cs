using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NomaNova.Ojeda.Web.Shared.Extensions
{
    public static class StringExtensions
    {
        private const int ShortHashLength = 7;
        
        public static string Truncate(this string value, int maxChars = Constants.DefaultTruncateSmall)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + (char)0x2026;
        }
        
        public static string ToHash(this string input)
        {
            return Sha1Hash(input ?? string.Empty);
        }
        
        public static string ToShortHash(this string input)
        {
            var hash = input.ToHash();
            return hash.Substring(0, ShortHashLength);
        }
        
        private static string Sha1Hash(string input)
        {
            using var sha1 = new SHA1Managed();

            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}