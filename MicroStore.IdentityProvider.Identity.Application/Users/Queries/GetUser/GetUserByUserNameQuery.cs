using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Queries.GetUser
{
    public class GetUserByUserNameQuery : IQuery<IdentityUserDto>
    {
        public string UserName { get; set; }
    }

    public class GetUserByUserNameQueryHandler : QueryHandler<GetUserByUserNameQuery, IdentityUserDto>
    {

        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public GetUserByUserNameQueryHandler(UserManager<ApplicationIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with user name : {request.UserName} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }
    }
}
