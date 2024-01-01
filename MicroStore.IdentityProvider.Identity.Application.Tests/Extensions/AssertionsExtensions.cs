using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using System.Data;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Extensions
{
    public static class AssertionsExtensions
    {
        public static void AssertUser(this ApplicationIdentityUser identityUser, UserModel model)
        {
            identityUser.Email.Should().Be(model.Email);
            identityUser.PhoneNumber.Should().Be(model.PhoneNumber);
            identityUser.GivenName.Should().Be(model.GivenName);
            identityUser.FamilyName.Should().Be(model.FamilyName);
            identityUser.UserRoles.Select(x => x.Role.Name).OrderBy(x=> x).Should().Equal(model.UserRoles?.OrderBy(x=>x), (left, right) => left == right);
            
        }

        public static async void AssertUserPassword(this ApplicationIdentityUser identityUser, UserModel model, IServiceProvider serviceProvider)
        {
            var applicationUserManager = serviceProvider.GetRequiredService<UserManager<ApplicationIdentityUser>>();

            var result =  await applicationUserManager.CheckPasswordAsync(identityUser, model.Password);

            result.Should().BeTrue();
        }


        public static void AssertUserDto(this ApplicationIdentityUser identityUser, IdentityUserDto userDto)
        {
            identityUser.Id.Should().Be(userDto.Id);
            identityUser.Email.Should().Be(userDto.Email);
            identityUser.PhoneNumber.Should().Be(userDto.PhoneNumber);
            identityUser.GivenName.Should().Be(userDto.GivenName);
            identityUser.FamilyName.Should().Be(userDto.FamilyName);
            identityUser.UserName.Should().Be(userDto.UserName);
            identityUser.UserRoles.Select(x => x.Role).OrderBy(x=> x.Name).Should().Equal(userDto.UserRoles.OrderBy(x=> x.Name), (left, right) => left.Name == right.Name);
 
        }


        public static void AssertRole(this ApplicationIdentityRole identityRole, RoleModel model)
        {
            identityRole.Name.Should().Be(model.Name);
            identityRole.Description.Should().Be(model.Description);
         
        }


        public static void AssertRoleDto(this ApplicationIdentityRole identityRole, IdentityRoleDto roleDto)
        {
            identityRole.Id.Should().Be(roleDto.Id);
            identityRole.Name.Should().Be(roleDto.Name);
            identityRole.Description.Should().Be(roleDto.Description);
        }
    }
}
