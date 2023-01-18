namespace MicroStore.BuildingBlocks.Results.Http
{
    public abstract class  Envelope
    {
        public ErrorInfo Error { get; set; }
        public DateTime TimeGenerated { get; set; }


        public static  Envelope<T> Failure<T>(ErrorInfo error)
        {
            return new  Envelope<T>
            {
                Error = error,
                TimeGenerated = DateTime.UtcNow,
                Result = default(T)
            };
        }

        public static  Envelope<T> Success<T>(T result) 
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
