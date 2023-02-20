namespace MicroStore.BuildingBlocks.Results.Http
{
    public class ErrorInfo
    {
        public string Type { get; set; }
        public string Message { get; set; }     
        public Dictionary<string, string[]> Errors { get; set; }


        public static ErrorInfo Create(string type , string message, Dictionary<string, string[]> errors = null)
        {
            return new ErrorInfo
            {
                Type = type,
                Message = message,
                Errors = errors
            };
        }

        public static ErrorInfo Validation(string message , Dictionary<string, string[]> errors = null)
        {
            return Create(HttpErrorType.ValidationError, message, errors);
        }
        public static ErrorInfo BusinessLogic(string message )
        {
            return Create(HttpErrorType.BusinessLogicError, message);
        }

        public static ErrorInfo NotFound(string message)
        {
            return Create(HttpErrorType.NotFoundError, message);
        }

        public static ErrorInfo UnAuthenticated(string message)
        {
            return Create(HttpErrorType.UnAuthenticatedError, message);    
        }
    
        public static ErrorInfo UnAuthorizedError(string message )
        {
            return Create(HttpErrorType.UnAuthorizedError, message);   
        }
        public static ErrorInfo BadGateway(string message )
        {
            return Create(HttpErrorType.BadGatewayError,message);
        }

    }



    public class HttpErrorType
    {
        public const string ValidationError = "validation_error";

        public const string BusinessLogicError = "businesslogic_error";

        public const string BadGatewayError = "badgateway_error";

        public const string NotFoundError = "notfound_error";

        public const string UnAuthenticatedError = "unauthenticated_error";

        public const string UnAuthorizedError = "unauthorized_error";

    }
}
