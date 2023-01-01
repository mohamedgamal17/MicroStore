namespace MicroStore.BuildingBlocks.Results.Http
{
    public class  Envelope
    {
        public ErrorInfo Error { get; set; }
        public DateTime TimeGenerated { get; set; }


        public static  Envelope Success()
        {
            return new  Envelope()
            {
                TimeGenerated = DateTime.UtcNow
            };
        }


        public static  Envelope Failure(ErrorInfo error)
        {
            return new  Envelope
            {
                Error = error,
                TimeGenerated = DateTime.UtcNow
            };
        }

        public static  Envelope Success<T>(T result) 
        {
            return new Envelope<T>
            {
                Result = result,
                TimeGenerated = DateTime.UtcNow
            };
        }

    }


    public class Envelope<T> :  Envelope
    {
        public T  Result { get; set; }

    }
}
