﻿using System.Security.Claims;
namespace MicroStore.BuildingBlocks.Security
{
    public interface IApplicationCurrentUser
    {
        bool IsAuthenticated { get; }

        string Id { get; }

        string UserName { get; }

        string Name { get; }

        string SurName { get; }

        string? PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        string? Email { get; }

        bool EmailVerified { get; }

        string[] Roles { get; }

        Claim? FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
