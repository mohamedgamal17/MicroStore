using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Interfaces;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Queries.GetUserList
{
    public class GetUserListQuery : PagingQueryParams ,IQuery<PagedResult<IdentityUserListDto>>
    { 
    }
    internal class GetUserListQueryHandler : QueryHandler<GetUserListQuery, PagedResult<IdentityUserListDto>>
    {
        private readonly IApplicationIdentityDbContext _identityDbContext;

        public GetUserListQueryHandler(IApplicationIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public override async Task<ResponseResult<PagedResult<IdentityUserListDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var query = _identityDbContext.Users
                .AsNoTracking()
                .ProjectTo<IdentityUserListDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.PageResult(request.PageNumber, request.PageSize,cancellationToken);

            return Success(HttpStatusCode.OK,result);
        }
    }
}
