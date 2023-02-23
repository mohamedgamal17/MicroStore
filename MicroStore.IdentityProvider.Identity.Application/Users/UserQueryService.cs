using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public class UserQueryService : IdentityApplicationService, IUserQueryService
    {

        private readonly IApplicationIdentityDbContext _identityDbContext;

        private readonly IIdentityUserRepository _identityUserRepository;
        public UserQueryService(IApplicationIdentityDbContext identityDbContext, IIdentityUserRepository identityUserRepository)
        {
            _identityDbContext = identityDbContext;
            _identityUserRepository = identityUserRepository;
        }

        public async Task<UnitResultV2<IdentityUserDto>> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _identityUserRepository.FindById(userId);

            if (user == null)
            {
                return UnitResultV2.Failure<IdentityUserDto>(ErrorInfo.NotFound($"User with id : {userId} is not exist"));
            }

            return UnitResultV2.Success(ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }

        public async Task<UnitResultV2<IdentityUserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _identityUserRepository.FindByEmail(email);

            if (user == null)
            {
                return UnitResultV2.Failure<IdentityUserDto>(ErrorInfo.NotFound($"User with email : {email} is not exist"));
            }

            return UnitResultV2.Success(ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }

        public async Task<UnitResultV2<IdentityUserDto>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _identityUserRepository.FindByUserName(userName);

            if (user == null)
            {
                return UnitResultV2.Failure<IdentityUserDto>(ErrorInfo.NotFound($"User with user name : {userName} is not exist"));
            }

            return UnitResultV2.Success(ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }

        public async Task<UnitResultV2<PagedResult<IdentityUserListDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _identityDbContext.Users
               .AsNoTracking()
               .ProjectTo<IdentityUserListDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResultV2.Success(result);
        }
    }
}
