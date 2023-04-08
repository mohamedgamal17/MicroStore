﻿using MassTransit;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Catalog.IntegrationEvents.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Threading;
namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class ProductUpdatedEventHandler : ILocalEventHandler<EntityUpdatedEventData<Product>> , ITransientDependency
    {
        private readonly IPublishEndpoint _publishEndPoint;

        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        public ProductUpdatedEventHandler(IPublishEndpoint publishEndPoint, ICancellationTokenProvider cancellationTokenProvider)
        {
            _publishEndPoint = publishEndPoint;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public Task HandleEventAsync(EntityUpdatedEventData<Product> eventData)
        {
            var integrationEvent = new ProductUpdatedIntegerationEvent
            {
                ProductId = eventData.Entity.Id.ToString(),
                Name = eventData.Entity.Name,
                Sku = eventData.Entity.Sku,
                Description = eventData.Entity.ShortDescription,
                UnitPrice = eventData.Entity.Price
            };
            if (eventData.Entity.ProductImages != null)
            {
                integrationEvent.ProductImages = eventData.Entity.ProductImages.Select(x => new ProductImageModel
                {
                    ImageLink = x.ImagePath,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }

            return _publishEndPoint.Publish(integrationEvent,_cancellationTokenProvider.Token);

        }
    }
}
