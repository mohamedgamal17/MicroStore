﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Domain.Entities;

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

        public async Task<Result<IdentityUserDto>> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _identityUserRepository.FindById(userId);

            if (user == null)
            {
                return new Result<IdentityUserDto>(new EntityNotFoundException(typeof(ApplicationIdentityUser), userId));
            }

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user);
        }

        public async Task<Result<IdentityUserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _identityUserRepository.FindByEmail(email);

            if (user == null)
            {
                return new Result<IdentityUserDto>(new EntityNotFoundException($"User with email : {email} is not exist"));
            }

            return   ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user);
        }

        public async Task<Result<IdentityUserDto>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _identityUserRepository.FindByUserName(userName);

            if (user == null)
            {
                return  new Result<IdentityUserDto>(new EntityNotFoundException($"User with user name : {userName} is not exist"));
            }

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user);
        }

        public async Task<Result<PagedResult<IdentityUserDto>>> ListAsync(UserListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var query = _identityDbContext.Users
               .AsNoTracking()
               .ProjectTo<IdentityUserDto>(MapperAccessor.Mapper.ConfigurationProvider);


            if (!string.IsNullOrEmpty(queryParams.UserName))
            {
                query = query.Where(x => x.UserName.Contains(queryParams.UserName));
            }

            if (!string.IsNullOrEmpty(queryParams.Role))
            {
                query = query.Where(x => x.UserRoles.Any(x => x.Name == queryParams.Role));
            }

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return result;
        }
    }
}
