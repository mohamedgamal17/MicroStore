using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{

    public class GetUserListQuery : PagingQueryParams, IQuery<PagedResult<IdentityUserListDto>>
    {

    }

    public class GetUserByEmailQuery : IQuery<IdentityUserDto>
    {
        public string Email { get; set; }
    }

    public class GetUserByIdQuery : IQuery<IdentityUserDto>
    {
        public string UserId { get; set; }
    }
    public class GetUserByUserNameQuery : IQuery<IdentityUserDto>
    {
        public string UserName { get; set; }
    }

}
