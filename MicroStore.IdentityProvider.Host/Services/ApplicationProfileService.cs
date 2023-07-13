﻿using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Host.Services
{
    public class ApplicationProfileService : DefaultProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationIdentityUser> _userClaimsPrincipalFactory;

        private readonly ApplicationUserManager _applicationUserManager;
        public ApplicationProfileService(ILogger<ApplicationProfileService> logger, ApplicationUserManager applicationUserManager, IUserClaimsPrincipalFactory<ApplicationIdentityUser> userClaimsPrincipalFactory) : base(logger)
        {
            _applicationUserManager = applicationUserManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }


        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);

            Logger.LogDebug("User Claims {Claims}", context.Subject.Claims);

            Logger.LogInformation("Requested Claims {Claims}", context.RequestedClaimTypes);

            var user = await _applicationUserManager.FindByIdAsync(context.Subject.GetSubjectId());

            var bol = _applicationUserManager.SupportsUserRole;

            var claimPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);

            context.AddRequestedClaims(claimPrincipal.Claims);

            context.LogIssuedClaims(Logger);

            

        }
    }
}
