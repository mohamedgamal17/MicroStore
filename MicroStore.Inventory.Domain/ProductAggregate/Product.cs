﻿#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
using Ardalis.GuardClauses;
using MicroStore.Inventory.Domain.Events;
using MicroStore.Inventory.Domain.Extensions;
using Volo.Abp;
using MicroStore.BuildingBlocks.Utils.Results;

namespace MicroStore.Inventory.Domain.ProductAggregate
{
    public class Product : BasicAggregateRoot<string>
    {
   
        public string Sku { get; set; }
        public string Name { get; set; }
        public string? Thumbnail { get; set; }
        public int Stock { get; private set; }
        public int AllocatedStock { get; private set; }
        public Product(string id)
        {
            Id = id;
        }

        public Product(string id ,string name , string sku , int stock)
        {
            Id = id;
            Name = name;
            Sku = sku;
            Stock = stock;
        }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }


        public void UpdateInventory(int stock)
        {
            Stock = stock;
        }


        public void AllocateStock(int quantity)
        {
            Guard.Against.InvalidResult(CanAllocateStock(quantity),typeof(Product));

            AddLocalEvent(new ProductAllocatedEvent
            {
                ProductId = Id,
                AllocatedQuantity = quantity,
                AllocationDate = DateTime.UtcNow
            });

            AllocatedStock += quantity;

            Stock -= quantity;
        }

        public Result<Unit> CanAllocateStock(int quantity)
        {
            if (Stock < quantity)
            {
                return new Result<Unit>(new BusinessException(
                     $"Current product : {Name} \n \t stock is less than requested allocated quantity"));
            }

            return Unit.Value;
        }

   
        public void ReleaseStock(int quantity)
        {        
            Guard.Against.InvalidResult(CanReleaseStock(quantity),typeof(Product));

            AddLocalEvent(new ProductReleasedEvent
            {
                ProductId = Id,
                ReleasedQuantity = quantity,
                ReleasedAt = DateTime.UtcNow
            });

            AllocatedStock -= quantity;

            Stock += quantity;
        }


        public Result<Unit> CanReleaseStock(int quantity)
        {
            if (AllocatedStock < quantity)
            {
                return new Result<Unit>(new BusinessException("Current allocated quantity is less than requested release quantity"));
            }

            return Unit.Value;
        }
       
    }
}
