namespace MicroStore.BuildingBlocks.Results.Http
{
    public class ValidationErrorInfo
    {
        public string[] Messages { get; set; }

        public string Field { get; set; }

        public ValidationErrorInfo()
        {

        }     
    }
}
