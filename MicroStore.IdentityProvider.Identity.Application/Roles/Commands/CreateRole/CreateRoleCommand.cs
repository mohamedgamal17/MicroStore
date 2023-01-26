using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : ICommand<IdentityRoleDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


    internal class CreateRoleCommandHandler : CommandHandler<CreateRoleCommand, IdentityRoleDto>
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public CreateRoleCommandHandler(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityRoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = new ApplicationIdentityRole
            {
                Name = request.Name,
                Description = request.Description
            };

            var identityResult = await _roleManager.CreateAsync(role);

            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.Created,ObjectMapper.Map<ApplicationIdentityRole,IdentityRoleDto>(role));
        }

        private ErrorInfo SerializeIdentityResultErrors(IdentityResult identityResult)
        {
            return new ErrorInfo
            {
                Message = "Error while creating role see validation error for more details",
                ValidationErrors = identityResult.Errors.Select(x => new ValidationErrorInfo
                {
                    Message = x.Description
                }).ToArray()
            };
        }
    }
}
