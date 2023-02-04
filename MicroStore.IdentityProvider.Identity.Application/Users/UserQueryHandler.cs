using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Interfaces;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public class UserQueryHandler : RequestHandler,
        IQueryHandler<GetUserListQuery, PagedResult<IdentityUserListDto>>,
        IQueryHandler<GetUserByEmailQuery, IdentityUserDto>,
        IQueryHandler<GetUserByUserNameQuery,IdentityUserDto>,
        IQueryHandler<GetUserByIdQuery, IdentityUserDto>
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IApplicationIdentityDbContext _identityDbContext;
        public UserQueryHandler(ApplicationUserManager applicationUserManager, IApplicationIdentityDbContext identityDbContext)
        {
            _applicationUserManager = applicationUserManager;
            _identityDbContext = identityDbContext;
        }

        public async Task<ResponseResult<PagedResult<IdentityUserListDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var query = _identityDbContext.Users
                .AsNoTracking()
                .ProjectTo<IdentityUserListDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<IdentityUserDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.NotFound, $"User with email : {request.Email} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }

        public async Task<ResponseResult<IdentityUserDto>> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.NotFound, $"User with user name : {request.UserName} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }

        public async Task<ResponseResult<IdentityUserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _identityDbContext.Users
                 .AsNoTracking()
                 .ProjectTo<IdentityUserDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (result == null)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

       
    }
}
