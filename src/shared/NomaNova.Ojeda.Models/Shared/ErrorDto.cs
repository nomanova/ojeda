using System.Collections.Generic;

namespace NomaNova.Ojeda.Models.Shared
{
    public class ErrorDto
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public Dictionary<string, List<string>> ValidationErrors { get; set; }
    }
}