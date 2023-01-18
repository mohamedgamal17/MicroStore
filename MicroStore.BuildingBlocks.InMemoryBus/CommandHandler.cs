using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Uow;
namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class CommandHandler<TCommand,TResponse> : RequestHandler<TCommand, TResponse> , IUnitOfWorkEnabled
        where TCommand : ICommand<TResponse>
    {
        
    }

    public abstract class CommandHandler<TCommand> : CommandHandler<TCommand, Unit>
        where TCommand : ICommand
    {

    }
}
