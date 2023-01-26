using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;
using System.Security.Claims;
namespace MicroStore.IdentityProvider.Identity.Application.Users.Commands.AssignUserIdentityClaims
{
    public class AssignUserIdentityClaimsCommand : ICommand<IdentityUserDto>
    {
        public Guid UserId { get; set; }

        public List<IdentityClaimModel> Claims { get; set; }
    }


    public class AssignUserIdentityClaimsCommandHandler : CommandHandler<AssignUserIdentityClaimsCommand, IdentityUserDto>
    {
        private readonly UserManager<ApplicationIdentityUser> _applicationUserManager;

        public AssignUserIdentityClaimsCommandHandler(UserManager<ApplicationIdentityUser> applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(AssignUserIdentityClaimsCommand request, CancellationToken cancellationToken)
        {

            var user = await _applicationUserManager.FindByIdAsync(request.UserId.ToString());
            var claims = request.Claims.Select(x => new Claim(x.Type, x.Value)).ToList();
            if (user == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            user.AddUserClaims(claims);

            var identityResult = await _applicationUserManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.OK,
                ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(await _applicationUserManager.FindByIdAsync(request.UserId.ToString())));
        }
    }
}
