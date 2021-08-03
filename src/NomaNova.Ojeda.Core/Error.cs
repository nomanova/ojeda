namespace NomaNova.Ojeda.Core
{
    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
    
    public class ValidationError : Error
    {
        public string Property { get; set; }
    }
}