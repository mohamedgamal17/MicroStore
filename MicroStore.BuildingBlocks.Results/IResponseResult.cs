namespace MicroStore.BuildingBlocks.Results
{
    public interface IResponseResult : IResult
    {
        public string Code { get; }

    }
    public interface IResponseResult<T> : IResponseResult, IResult<T>
    {

    }
}
