using System;
using System.Linq;

namespace NomaNova.Ojeda.Api.Tests.Helpers
{
    public static class FactoryHelper
    {
        private static readonly Random Random = new();

        public static string RandomString(int length = 10)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}