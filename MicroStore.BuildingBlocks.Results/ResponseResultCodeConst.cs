namespace MicroStore.BuildingBlocks.Results
{
    public static class ResponseResultCodeConst
    {
        public static string Ok => "ok";
        public static string Created => "created";
        public static string Updated => "updated";
        public static string Deleted => "deleted";
        public static string NotFound => "not-found";
        public static string ValidationError => "validation-error";
        public static string BusniessLogicErorr => "busniess-logic-error";
        public static string UnAuthonticated => "unauthonticated";
        public static string UnAuthorized => "unauthorized";
        public static string UnHandledException => "unhandled-exception";
    }
}
