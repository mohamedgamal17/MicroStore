namespace MicroStore.BuildingBlocks.Results.Http
{
    public class ValidationErrorInfo
    {
        public string Message { get; set; }

        public string[] Members { get; set; }

        public ValidationErrorInfo()
        {

        }

        public ValidationErrorInfo(string message)
        {
            Message = message;
        }

        public ValidationErrorInfo(string message, string[] members)
            : this(message)
        {
            Members = members;
        }


        public ValidationErrorInfo(string message, string member)
            : this(message, new[] { member })
        {

        }
    }
}
