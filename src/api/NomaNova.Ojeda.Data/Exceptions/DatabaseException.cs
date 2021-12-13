using System;

namespace NomaNova.Ojeda.Data.Exceptions
{
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException()
        {
        }

        public DatabaseException(string message) : base(message)
        {
        }
    }
}