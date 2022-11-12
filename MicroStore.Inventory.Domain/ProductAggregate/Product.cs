using Volo.Abp.Domain.Entities;
using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Domain.Events;
using MicroStore.Inventory.Domain.Extensions;

namespace MicroStore.Inventory.Domain.ProductAggregate
{
    public class Product : AggregateRoot<Guid>
    {

        public int Stock { get; private set; }
        public int AllocatedStock { get; private set; }


        public Product(int stock)
            : base(Guid.NewGuid())
        {
            Stock = Guard.Against.Negative(stock, nameof(stock));
        }


        public void AdjustInventory(int adjustedStock, string reason)
        {
            Guard.Against.Negative(adjustedStock, nameof(adjustedStock));

            AddLocalEvent(new InventoryAdjustedEvent
            {
                ProductId = Id,
                AdjustedQuantity = adjustedStock - Stock,
                AdkustedDate = DateTime.UtcNow,
                Reason = reason
            });

            Stock = adjustedStock;
        }


        public void AllocateStock(int allocatedQuantity)
        {
            Guard.Against.InvalidResult(CanAllocateQuantity(allocatedQuantity));

            AddLocalEvent(new ProductAllocatedEvent
            {
                ProductId = Id,
                AllocatedQuantity = allocatedQuantity,
                AllocationDate = DateTime.UtcNow
            });

            AllocatedStock += allocatedQuantity;

            Stock -= allocatedQuantity;
        }

        public Result CanAllocateQuantity(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            return Result.FailureIf(() => Stock < quantity, "Current product stock is less than requested allocated quantity");
        }


        public void ReleaseStock(int quantity)
        {
            Guard.Against.InvalidResult(CanReleaseStock(quantity));

            AddLocalEvent(new ProductReleasedEvent
            {
                ProductId = Id,
                ReleasedQuantity = quantity,
                ReleasedAt = DateTime.UtcNow
            });

            AllocatedStock -= quantity;
            Stock += quantity; 
        }

        public Result CanReleaseStock(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            return Result.FailureIf(() => AllocatedStock < quantity, "Current allocated quantity is less than requested release quantity");
        }

        public void ShipProduct(int quantity)
        {
            Guard.Against.InvalidResult(CanShipProduct(quantity));

            AllocatedStock -= quantity;

            AddLocalEvent(new ProductShippedEvent
            {
                ProductId = Id,
                ShippedQuantity = quantity,
                ShippedDate = DateTime.UtcNow
            });
        }

        public Result CanShipProduct(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            return Result.FailureIf(() => AllocatedStock < quantity, "Current allocated quantity is less than requested ship quantity");
        }


        public void ReciveQuantity(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity,nameof(quantity));


            Stock += quantity;

            AddLocalEvent(new ProductRecivedEvent
            {
                ProductId = Id,
                RecivedQuantity = quantity,
                RecivedDate = DateTime.UtcNow
            });
        }



        public void ReturnQuantity(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            Stock += quantity;

            AddLocalEvent(new ProductReturnedEvent
            {
                ProductId = Id,
                ReturnedQuantity = quantity,
                ReturnedDate = DateTime.UtcNow
            });
        }

    }
}
