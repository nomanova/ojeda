namespace NomaNova.Ojeda.Api.Models
{
    public enum ErrorType
    {
        General
    }
    
    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }
        
        public static Error General(string message)
        {
            return FromType(ErrorType.General, message);
        }

        public static Error FromType(ErrorType type, string message = null)
        {
            var error = new Error
            {
                Code = type.ToString(), 
                Message = message
            };

            return error;
        }
    }
}