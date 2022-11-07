using AutoMapper;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Application.Abstraction.Queries;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace MicroStore.ShoppingCart.Application.Queries
{
    internal class GetBasketQueryHandler : QueryHandler<GetBasketQuery, BasketDto>
    {

        private readonly ICurrentUser _currentUser;

        private readonly IRepository<Basket> _basketRepository;
        public GetBasketQueryHandler(ICurrentUser currentUser, IMapper mapper, IRepository<Basket> basketRepository)
        {

            _currentUser = currentUser;
            _basketRepository = basketRepository;
        }


        public override async Task<BasketDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.Id == request.BasketId);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }
    }
}
