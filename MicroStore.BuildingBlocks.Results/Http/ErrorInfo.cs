namespace MicroStore.BuildingBlocks.Results.Http
{
    public class ErrorInfo
    {
        public string Message { get; set; }

        public string Details { get; set; }
        public ValidationErrorInfo[] ValidationErrors { get; set; }

    }
}
