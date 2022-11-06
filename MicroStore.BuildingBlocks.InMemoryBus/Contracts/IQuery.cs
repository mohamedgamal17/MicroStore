using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {

    }
}
