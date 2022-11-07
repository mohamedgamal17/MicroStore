using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp;

namespace MicroStore.ShoppingCart.Application.Commands
{
    public class CreateBaskeCommandHandler : CommandHandler<CreateBasketCommand, BasketDto>
    {
        private readonly ICurrentUser _currentUser;

        private readonly IRepository<Basket> _basketRepository;

        public CreateBaskeCommandHandler(ICurrentUser currentUser,
            IRepository<Basket> basketRepository)
        {
            _currentUser = currentUser;
            _basketRepository = basketRepository;
        }

        public override async Task<BasketDto> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {

            bool userHasBasket = await _basketRepository.AnyAsync(x => x.UserId == _currentUser.Id);

            if (userHasBasket)
            {
                throw new UserFriendlyException("User has already basket");
            }

            Basket basket = new Basket(_currentUser.Id!.Value);

            await _basketRepository.InsertAsync(basket);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }
    }
}
