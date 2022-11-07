using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.ShoppingCart.Application.Commands
{
    public class UpdateBasketItemQuantityCommandHandler : CommandHandler<UpdateBasketItemQuantityCommand, BasketDto>
    {

        private readonly IRepository<Basket> _basketRepository;


        public UpdateBasketItemQuantityCommandHandler(IRepository<Basket> basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public override async Task<BasketDto> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
        {
            Basket? basket = await _basketRepository.SingleOrDefaultAsync(x => x.Id == request.BasketId);

            if (basket == null)
            {
                throw new EntityNotFoundException(typeof(Basket));
            }

            if (!basket.IsBasketItemExist(request.BasketItemId))
            {
                throw new EntityNotFoundException(typeof(BasketItem), request.BasketItemId);
            }

            basket.UpdatetBasketItemQuantity(request.BasketItemId, request.Quantity);


            await _basketRepository.UpdateAsync(basket);


            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }
    }
}
