using MicroStore.BuildingBlocks.Results;
namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {

    }


    public interface IQuery : IQuery<ResponseResult>
    {
        
    }

}
