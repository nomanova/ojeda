namespace NomaNova.Ojeda.Models
{
    public class ErrorDto
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
    
    public class ValidationErrorDto : ErrorDto
    {
        public string Property { get; set; }
    }
}