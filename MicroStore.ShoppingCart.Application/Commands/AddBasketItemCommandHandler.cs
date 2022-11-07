using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace MicroStore.ShoppingCart.Application.Commands
{
    internal class AddBasketItemCommandHandler : CommandHandler<AddBasketItemCommand, BasketDto>
    {

        private readonly IRepository<Basket> _basketRepository;

        private readonly IRepository<Product> _productRepository;

        private readonly ICurrentUser _currentUser;

        public AddBasketItemCommandHandler(IRepository<Basket> basketRepository, ICurrentUser currentUser, IRepository<Product> productRepository)
        {
            _basketRepository = basketRepository;
            _currentUser = currentUser;
            _productRepository = productRepository;
        }

        public override async Task<BasketDto> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {

            Basket? basket = await _basketRepository
              .SingleOrDefaultAsync(x => x.Id == request.BasketId);

            if (basket == null)
            {
                throw new EntityNotFoundException(typeof(Basket), request.BasketId);
            }

            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }

            basket.AddItem(product, request.Quantity);

            await _basketRepository.UpdateAsync(basket);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }



    }
}
