

namespace MicroStore.Inventory.Application.Abstractions.Common
{
    public interface ICurrentState
    {
        ICurrentState DeepCopy();
    }
}
