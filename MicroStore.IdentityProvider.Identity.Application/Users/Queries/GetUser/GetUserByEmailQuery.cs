using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Queries.GetUser
{
    public class GetUserByEmailQuery : IQuery<IdentityUserDto>
    {
        public string Email { get; set; }
    }
    public class GetUserByEmailQueryHandler : QueryHandler<GetUserByEmailQuery, IdentityUserDto>
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public GetUserByEmailQueryHandler(UserManager<ApplicationIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with email : {request.Email} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityUser,IdentityUserDto>(user));
        }
    }
}
