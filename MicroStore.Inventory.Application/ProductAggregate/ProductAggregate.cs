using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.Application.ProductAggregate.Exceptions;
using MicroStore.Inventory.Events;
using MicroStore.Inventory.Events.Contracts;

namespace MicroStore.Inventory.Application.ProductAggregate
{

    public class Product : AggregateRoot
    {

        private CurrentState _currentState;

        public Product(Guid correlationId) 
            : base(correlationId)
        {
            _currentState = new CurrentState();
        }

        protected override void ApplyEvent(IEvent domainEvent)
        {
            switch (domainEvent)
            {
                case InventoryAdjustedEvent inventoryAdjusted:
                    Apply(inventoryAdjusted);
                    break;
                case ProductRecivedEvent productRecived:
                    Apply(productRecived);
                    break;
                case ProductShippedEvent productShipped:
                    Apply(productShipped);
                    break;
                case ProductAllocatedEvent productAllocated:
                    Apply(productAllocated);
                    break;
                case ProductReturnedEvent productReturned:
                    Apply(productReturned);
                    break;
                case ProductReleasedEvent productReleased:
                    Apply(productReleased);
                    break;
                case ProductDispatchedEvent productDispatchedEvent:
                    break;
                default:
                    throw new InvalidOperationException("Not supported domain event");

            }
        }

       
        public override ICurrentState GetCurrentState()
        {
            return _currentState.DeepCopy();
        }

        public void Dispatch(string name ,string sku)
        {
            Append(new ProductDispatchedEvent (name, sku));
        }

        public void AdJustInventory(int quantity,string reason , DateTime adjustedDate)
        {
            Result result = CanAdustInvetory(quantity);

            if (result.IsFailure)
            {
                throw new InvalidDomainException(result.Error);
            }

            Append(new InventoryAdjustedEvent(quantity, reason, adjustedDate));
        }
        public Result CanAdustInvetory(int quantiy)
        {
            return Result.FailureIf(() => (_currentState.StockOnHand + quantiy) < 0 , "Cannot adjust product quantity");
        }


        public void AllocateStock(int stock, DateTime allocationDate)
        {
            Result result = CanAllocateStock(stock);

            if (result.IsFailure)
            {
                throw new InvalidDomainException(result.Error);
            }

            Append(new ProductAllocatedEvent(stock, allocationDate));
        }

        public Result CanAllocateStock(int stock)
        {
            return Result.FailureIf(() => _currentState.StockOnHand < stock, "Product stock is less than requested allocate quantity");
        }
        public void ReleaseStock(int stock,DateTime releasedDate)
        {
            Result result = CanReleaseStock(stock);

            if (result.IsFailure)
            {
                throw new InvalidDomainException(result.Error);
            }

            Append(new ProductReleasedEvent(stock, releasedDate));
        }

        public Result CanReleaseStock(int stock)
        {
            return Result.FailureIf(() => _currentState.AllocatedStock < stock, "Allocated stock is les than requested release quantity");
        }

        public void ShipProduct(int stock, DateTime shippedDate)
        {
            Result result = CanShipProduct(stock);

            if (result.IsFailure)
            {
                throw new InvalidDomainException(result.Error);
            }

            Append(new ProductShippedEvent(stock, shippedDate));
        }

        public Result CanShipProduct(int stock)
        {
            return Result.FailureIf(() => _currentState.AllocatedStock < stock, "Allocated stock is less than requested ship quantity");
        }

        public void ReciveProduct(int stock, DateTime recivedDate)
        {
            Append(new ProductRecivedEvent(stock, recivedDate));
        }

        public override void Recive(ICurrentState currentState)
        {
            if(currentState.GetType() != typeof(CurrentState))
            {
                throw new InvalidOperationException($"Invalid Product Current State : {currentState.GetType().AssemblyQualifiedName}");
            }

            _currentState = (CurrentState) currentState;
        }

        public void ReturnStock(int stock , DateTime returnedDate)
        {
            Append(new ProductReturnedEvent(stock, returnedDate));
        }


        private void Apply(InventoryAdjustedEvent domainEvent)
        {
            _currentState.StockOnHand += domainEvent.AdjustedQuantity;
        }

        private void Apply(ProductRecivedEvent productRecived)
        {
            _currentState.StockOnHand = productRecived.RecivedQuantity;
        }

        private void Apply(ProductAllocatedEvent productAllocated)
        {
            _currentState.StockOnHand -= productAllocated.AllocatedQuantity;
            _currentState.AllocatedStock = productAllocated.AllocatedQuantity;
        }

        private void Apply(ProductShippedEvent productShippedEvent)
        {
            _currentState.AllocatedStock -= productShippedEvent.ShippedQuantity;
        }

        private void Apply(ProductReleasedEvent productReleased)
        {
            _currentState.AllocatedStock -= productReleased.ReleasedQuantity;
            _currentState.StockOnHand += productReleased.ReleasedQuantity;
        }

        private void Apply(ProductReturnedEvent productReturned)
        {
            _currentState.StockOnHand += productReturned.ReturnedQuantity;
        }


        private class CurrentState : ICurrentState
        {
            public int StockOnHand { get; set; }
            public int AllocatedStock { get; set; }


            public ICurrentState DeepCopy()
            {
                return new CurrentState
                {
                    StockOnHand = StockOnHand,
                    AllocatedStock = AllocatedStock
                };
            }

        }

    }
}
