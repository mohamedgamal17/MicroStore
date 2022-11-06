using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface IRequestPostProcess<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);

    }
}
