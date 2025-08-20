using System.Diagnostics.CodeAnalysis;

namespace API_Simulacao_Hack.Util.Base
{
    [ExcludeFromCodeCoverage]
    public class CustomException: Exception 
    {
        public CustomException() { }
        public CustomException(string message) : base(message) { }
        public CustomException(string message,  Exception innerException) : base(message, innerException) { }

    }
}
