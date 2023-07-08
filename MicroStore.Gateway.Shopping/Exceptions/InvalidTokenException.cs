namespace MicroStore.Gateway.Shopping.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string error, string errorType, string errorDesceription)
        {
            Error = error;
            ErrorType = errorType;
            ErrorDesceription = errorDesceription;
        }

        public string Error { get; set; }
        public string ErrorType { get; set; }
        public string  ErrorDesceription { get; set; }
    }
}
