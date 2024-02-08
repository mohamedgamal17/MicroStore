using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Profiling.Application.Domain;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.EntityFramewrok;
using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Profiling.Application.Services
{
    public class ProfileQueryService : ProfilingApplicationService, IProfileQueryService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IMapperAccessor _mapperAccessor;
        public ProfileQueryService(ApplicationDbContext applicationDbContext, IMapperAccessor mapperAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _mapperAccessor = mapperAccessor;
        }

        public async Task<Result<PagedResult<ProfileDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {

            var query = _applicationDbContext.Profiles
                .AsNoTracking()
                .ProjectTo<ProfileDto>(_mapperAccessor.Mapper.ConfigurationProvider);



            return await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);
        }


        public async Task<Result<ProfileDto>> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            var query = _applicationDbContext.Profiles
                .AsNoTracking()
                .ProjectTo<ProfileDto>(_mapperAccessor.Mapper.ConfigurationProvider);


            var profile = await query.SingleOrDefaultAsync(x => x.UserId == userId);

            if(profile == null)
            {
                return new Result<ProfileDto>(new EntityNotFoundException(typeof(Profile), userId));
            }

            return profile;
        }

        public async Task<Result<List<AddressDto>>> ListAddressesAsync(string userId, CancellationToken cancellationToken = default)
        {
            var query = _applicationDbContext.Profiles
               .AsNoTracking()
               .ProjectTo<ProfileDto>(_mapperAccessor.Mapper.ConfigurationProvider);


            var profile = await query.SingleOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
            {
                return new Result<List<AddressDto>>(new EntityNotFoundException(typeof(Profile), userId));
            }

            return profile.Addresses ?? new List<AddressDto>();
        }

        public async Task<Result<AddressDto>> GetAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default)
        {
            var query = _applicationDbContext.Profiles
             .AsNoTracking()
             .ProjectTo<ProfileDto>(_mapperAccessor.Mapper.ConfigurationProvider);


            var profile = await query.SingleOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
            {
                return new Result<AddressDto>(new EntityNotFoundException(typeof(Profile), userId));
            }

            var address = profile.Addresses?.SingleOrDefault(x => x.Id == addressId);

            if(address == null)
            {
                return new Result<AddressDto>(new EntityNotFoundException(typeof(Address), addressId));
            }

            return address;
        }

        public async Task<Result<List<ProfileDto>>> ListByIdsAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            if(ids != null && ids.Count > 0)
            {
                var query = _applicationDbContext.Profiles.AsNoTracking();


                var profiles = await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

                return ObjectMapper.Map<List<Profile>, List<ProfileDto>>(profiles);
            }

            return new List<ProfileDto>();
           
        }
    }
}
