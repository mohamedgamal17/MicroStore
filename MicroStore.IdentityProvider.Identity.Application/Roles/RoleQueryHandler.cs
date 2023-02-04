using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Interfaces;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using System.Net;
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleQueryHandler : RequestHandler,
          IQueryHandler<GetRoleListQuery, ListResultDto<IdentityRoleDto>>,
        IQueryHandler<GetRoleWithNameQuery, IdentityRoleDto>,
        IQueryHandler<GetRoleWithIdQuery,IdentityRoleDto>
    {
        private readonly ApplicationRoleManager _applicationRoleManager;

        private readonly IApplicationIdentityDbContext _identityDbContext;

        public RoleQueryHandler(ApplicationRoleManager applicationRoleManager, IApplicationIdentityDbContext identityDbContext)
        {
            _applicationRoleManager = applicationRoleManager;
            _identityDbContext = identityDbContext;
        }

        public async Task<ResponseResult<ListResultDto<IdentityRoleDto>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var query = _identityDbContext.Roles
           .AsNoTracking()
           .ProjectTo<IdentityRoleDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<IdentityRoleDto>> Handle(GetRoleWithNameQuery request, CancellationToken cancellationToken)
        {
            var role = await _applicationRoleManager.FindByNameAsync(request.Name);

            if (role == null)
            {
                return Failure<IdentityRoleDto>(HttpStatusCode.NotFound, $"Role with name : {request.Name} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role));
        }

        public async Task<ResponseResult<IdentityRoleDto>> Handle(GetRoleWithIdQuery request, CancellationToken cancellationToken)
        {

            var role = await _applicationRoleManager.FindByIdAsync(request.Id);

            if (role == null)
            {
                return Failure<IdentityRoleDto>(HttpStatusCode.NotFound, $"Role with id : {request.Id} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role));
        }
    }
}
