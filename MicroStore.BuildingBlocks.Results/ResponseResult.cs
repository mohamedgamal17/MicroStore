using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.BuildingBlocks.Results
{
    public  class ResponseResult 
    {
        public int StatusCode { get; }
        public bool IsSuccess { get;  }
        public bool IsFailure => !IsSuccess;
        public Envelope Envelope { get; }

        public ResponseResult(bool success,  int statusCode, Envelope envelope)
        {
            IsSuccess = success;
            StatusCode = statusCode;
            Envelope = envelope;
        }
        public Envelope<T> GetEnvelopeResult<T>()
        {
            var cast = Envelope as Envelope<T>;

            return cast ?? throw new InvalidOperationException($"Unable to cast envelope result from {Envelope.GetType().AssemblyQualifiedName}, to {(typeof(Envelope<T>).AssemblyQualifiedName)}");
        }

        public static ResponseResult Success(int code)
        {
            return new ResponseResult(true,code, Envelope.Success());
        }

        public static ResponseResult Failure(int code , ErrorInfo error)
        {
            return new ResponseResult(false,code , Envelope.Failure(error));
        }

        public static ResponseResult Success<T>(int code , T value)
        {
            return new ResponseResult(true,code, Envelope.Success(value));
        }    
    }
}
