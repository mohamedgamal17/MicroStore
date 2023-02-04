using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Application.Users;
using System.Data;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Extensions
{
    public static class AssertionsExtensions
    {
        public static void AssertUser(this ApplicationIdentityUser identityUser, UserCommand command)
        {
            identityUser.Email.Should().Be(command.Email);
            identityUser.PhoneNumber.Should().Be(command.PhoneNumber);
            identityUser.FirstName.Should().Be(command.FirstName);
            identityUser.LastName.Should().Be(command.LastName);

            identityUser.UserRoles.Select(x => x.Role.Name).OrderBy(x=> x).Should().Equal(command.UserRoles?.OrderBy(x=>x), (left, right) => left == right);
            identityUser.UserClaims.Should().Equal(command.UserClaims, (left, right) => left.ClaimType == right.Type && left.ClaimValue == right.Value);
        }

        public static async void AssertUserPassword(this ApplicationIdentityUser identityUser, UserCommand command, IServiceProvider serviceProvider)
        {
            var applicationUserManager = serviceProvider.GetRequiredService<UserManager<ApplicationIdentityUser>>();

            var result =  await applicationUserManager.CheckPasswordAsync(identityUser, command.Password);

            result.Should().BeTrue();
        }


        public static void AssertUserDto(this ApplicationIdentityUser identityUser, IdentityUserDto userDto)
        {
            identityUser.Id.Should().Be(userDto.Id);
            identityUser.Email.Should().Be(userDto.Email);
            identityUser.PhoneNumber.Should().Be(userDto.PhoneNumber);
            identityUser.FirstName.Should().Be(userDto.FirstName);
            identityUser.LastName.Should().Be(userDto.LastName);
            identityUser.UserName.Should().Be(userDto.UserName);
            identityUser.UserRoles.Select(x => x.Role).Should().Equal(userDto.UserRoles, (left, right) => left.Name == right.Name);
            identityUser.UserClaims.Should().Equal(userDto.UserClaims, (left, right) => left.ClaimType == right.ClaimType && left.ClaimValue == right.ClaimValue);
        }


        public static void AssertRole(this ApplicationIdentityRole identityRole, RoleCommand command)
        {
            identityRole.Name.Should().Be(command.Name);
            identityRole.Description.Should().Be(command.Description);
            identityRole.RoleClaims.Count.Should().Be(command.Claims.Count);
            identityRole.RoleClaims.Should().Equal(command.Claims, (left, right) => left.ClaimType == right.Type && left.ClaimValue == right.Value);
        }


        public static void AssertRoleDto(this ApplicationIdentityRole identityRole, IdentityRoleDto roleDto)
        {
            identityRole.Id.Should().Be(roleDto.Id);
            identityRole.Name.Should().Be(roleDto.Name);
            identityRole.Description.Should().Be(roleDto.Description);
            identityRole.RoleClaims.Count.Should().Be(roleDto.RoleClaims.Count);
            identityRole.RoleClaims.Should().Equal(roleDto.RoleClaims, (left, right) => left.ClaimType == right.ClaimType && left.ClaimValue == right.ClaimValue);
        }
    }
}
