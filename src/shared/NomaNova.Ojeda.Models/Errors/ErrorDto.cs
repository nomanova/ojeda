using System.Collections.Generic;

namespace NomaNova.Ojeda.Models.Errors
{
    public class ErrorDto
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public IEnumerable<ValidationErrorDto> Errors { get; set; }
    }
}