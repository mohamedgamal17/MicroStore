using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class ProductImageEventHandler : ILocalEventHandler<EntityCreatedEventData<ProductImage>>
    {
        private readonly IImageService _imageService;

        public ProductImageEventHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ProductImage> eventData)
        {
            var entity = eventData.Entity;

            await _imageService.IndexImageAsync(entity.ProductId, entity.ProductId, entity.ImagePath);
        }
    }
}
