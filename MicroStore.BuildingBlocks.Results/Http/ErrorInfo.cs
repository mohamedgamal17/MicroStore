namespace MicroStore.BuildingBlocks.Results.Http
{
    public class ErrorInfo
    {
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }     
        public string Details { get; set; }
        public ValidationErrorInfo[] ValidationErrors { get; set; }

    }
}
