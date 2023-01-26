using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Interfaces;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Queries.GetUser
{
    public class GetUserByIdQuery : IQuery<IdentityUserDto>
    {
        public Guid UserId { get; set; }
    }


    internal class GetUserByIdQueryHandler : QueryHandler<GetUserByIdQuery, IdentityUserDto>
    {
        private readonly IApplicationIdentityDbContext _identityDbContext;

        public GetUserByIdQueryHandler(IApplicationIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _identityDbContext.Users
                .AsNoTracking()
                .ProjectTo<IdentityUserDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
