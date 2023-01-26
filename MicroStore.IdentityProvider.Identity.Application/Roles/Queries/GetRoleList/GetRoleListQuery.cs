using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Interfaces;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.Identity.Application.Roles.Queries.GetRoleList
{
    public class GetRoleListQuery :IQuery<ListResultDto<IdentityRoleDto>>
    {
    }

    public class GetRoleListQueryHandler : QueryHandler<GetRoleListQuery, ListResultDto<IdentityRoleDto>>
    {
        private readonly IApplicationIdentityDbContext _identityDbContext;

        public GetRoleListQueryHandler(IApplicationIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public override async Task<ResponseResult<ListResultDto<IdentityRoleDto>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var query = _identityDbContext.Roles
                .AsNoTracking()
                .ProjectTo<IdentityRoleDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
    }
}
