
namespace MicroStore.BuildingBlocks.Results
{
    public abstract class Result
    {

        private bool _isSucess;
        private string _error;
        public bool IsSuccess => _isSucess;
        public bool IsFailure => !_isSucess;

        public string Error
        {
            get
            {
                if (IsFailure)
                {
                    return _error!;
                }

                throw new InvalidOperationException("result is already succeeded");
            }

        }

        protected Result(bool isSucess, string? error)
        {
            _isSucess = isSucess;
            _error = error;
        }

    }
   
}
