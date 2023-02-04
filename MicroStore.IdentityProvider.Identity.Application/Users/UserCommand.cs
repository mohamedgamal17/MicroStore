using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public abstract class UserCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public List<string> UserRoles { get; set; }
        public List<IdentityClaimModel> UserClaims { get; set; }

    }


    public class CreateUserCommand : UserCommand, ICommand<IdentityUserDto>
    {


    }


    public class UpdateUserCommand : UserCommand, ICommand<IdentityUserDto>
    {
        public string UserId { get; set; }
    }
}
