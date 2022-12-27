namespace MicroStore.BuildingBlocks.Results
{
    [Obsolete]
    public static class UnitResultCodeConst
    {
        public static string Ok => "ok";
        public static string Created => "created";
        public static string Accepted => "accepted";
        public static string Deleted => "deleted";
        public static string NotFound => "not_found";
        public static string ValidationError => "validation_error";
        public static string BusniessLogicErorr => "busniess_logic_error";
        public static string UnAuthonticated => "unauthonticated";
        public static string UnAuthorized => "unauthorized";
        public static string UnHandledException => "unhandled_exception";
    }
}
