using System.Security.Cryptography;
using System.Text;

namespace NomaNova.Ojeda.Utils.Extensions
{
    public static class StringExtensions
    {
        private const int ShortHashLength = 7;

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