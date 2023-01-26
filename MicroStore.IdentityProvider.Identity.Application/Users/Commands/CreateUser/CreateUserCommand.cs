using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICommand<IdentityUserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }


    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, IdentityUserDto>
    {

        private readonly UserManager<ApplicationIdentityUser> _applicationUserManager;

        public CreateUserCommandHandler(UserManager<ApplicationIdentityUser> applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = new ApplicationIdentityUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber   
            };


            var identityResult = await  _applicationUserManager.CreateAsync(applicationUser,request.Password);


            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }


            return Success(HttpStatusCode.Created, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser));
        }
      
    }
}

