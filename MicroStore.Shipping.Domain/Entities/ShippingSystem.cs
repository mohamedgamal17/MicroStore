﻿#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
namespace MicroStore.Shipping.Domain.Entities
{
    public class ShippingSystem : Entity<string>
    {
        public string  Name { get; set; }
        public string DisplayName { get; set; }
        public string  Image { get; set; }
        public bool IsEnabled { get; set; }

        public ShippingSystem()
        {
            Id = Guid.NewGuid().ToString();
         }
    }
}
