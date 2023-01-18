using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.BuildingBlocks.Results
{
    
    public class ResponseResult<T>
    {
        public int StatusCode { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public Envelope<T> EnvelopeResult { get; }

        public ResponseResult(bool success, int statusCode, Envelope<T> envelope)
        {
            IsSuccess = success;
            StatusCode = statusCode;
            EnvelopeResult = envelope;
        }
    }


    public class ResponseResult : ResponseResult<Unit>
    {
        public ResponseResult(bool success, int statusCode, Envelope<Unit> envelope)
            : base(success, statusCode, envelope)
        {

        }


        public static ResponseResult Success(int code)
        {
            return new ResponseResult(true, code, Envelope.Success(Unit.Value));
        }

        public static ResponseResult Failure(int code, ErrorInfo error)
        {
            return new ResponseResult(false, code, Envelope.Failure<Unit>(error));
        }

        public static ResponseResult<T> Success<T>(int code, T value)
        {
            return new ResponseResult<T>(true, code, Envelope.Success(value));
        }

        public static ResponseResult<T> Failure<T> (int code,ErrorInfo error)
        {
            return new ResponseResult<T>(false, code, Envelope.Failure<T>(error));
        }
    }

}
