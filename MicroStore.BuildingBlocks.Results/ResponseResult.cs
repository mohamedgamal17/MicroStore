namespace MicroStore.BuildingBlocks.Results
{
    public  class ResponseResult : Result, IResponseResult
    {
        public string Code { get; }

        internal ResponseResult(bool isSucces, string code ,object? error)
            :base(isSucces , error)
        {
            Code = code;
        }

        public ResponseResult Success(string code)
        {
            return new ResponseResult(true, code, string.Empty);
        }


        public ResponseResult Failure(string code, object error)
        {
            return new ResponseResult(false, code, error);
        }

        public ResponseResult<T> Success<T>(T value , string code)
        {
            return new ResponseResult<T>(true,value, code,string.Empty);
        }

        public ResponseResult<T> Failure<T> (string code, object error)
        {
            return new ResponseResult<T>(false, default(T), code, error);
        }

    }


    public class ResponseResult<T> : ResponseResult, IResponseResult<T>
    {

        public string Code { get; private set; }

        private T? _value;

        

        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException("result is already failured");
                }

                return _value!;
            }
        }

        internal ResponseResult(bool isSucces,T? value ,string code, object? error) 
            : base(isSucces, code, error)
        {
            _value = value;
            Code = code;
        }
    }
}
