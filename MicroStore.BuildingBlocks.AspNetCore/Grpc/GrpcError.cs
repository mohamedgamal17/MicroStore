namespace MicroStore.BuildingBlocks.AspNetCore.Grpc
{
    public class GrpcError
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<GrpcValidationError> Errors { get; set; } = new List<GrpcValidationError>();

        public GrpcError()
        {            
        }

        public static GrpcError ValidationErrors(List<GrpcValidationError> errors)
        {
            return new GrpcError
            {
                Code = GrpcErrorType.ValidationError,
                Title = "Validation error",
                Details = "One or more validation error occured",
                Errors = errors
            };
        }
        public static GrpcError BussniessLogicError(string message)
        {
            return new GrpcError
            {
                Code = GrpcErrorType.BussniessLogic,
                Title = "Invalid entity state",
                Details = message,
            };
        }

        public static GrpcError NotFoundError(string message)
        {
            return new GrpcError
            {
                Code = GrpcErrorType.NotFound,
                Title = "Entity not found",
                Details = message
            };
        }

        public static GrpcError InternalError(string messsage)
        {
            return new GrpcError
            {
                Code = GrpcErrorType.InternalError,
                Title = "Internal server error",
                Details = messsage
            };
        }



    }


    public class GrpcValidationError
    {
        public string Field { get; set; }
        public string[] Errors { get; set; }
    }
}
