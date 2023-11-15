﻿using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;

namespace MicroStore.AspNetCore.UI
{
    [ExposeServices(typeof(IWorkContext))]
    public class DefaultWorkContext : IWorkContext , ITransientDependency
    {
        const string AnonymousUser = "anonymous_userid";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultWorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TryToGetCurrentUserId()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.User.Identity?.IsAuthenticated == true)
            {
                return context.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            }


            string? userId;

            if (context?.Request.Cookies.TryGetValue(AnonymousUser, out userId) == true && userId != null)
            {
                return userId;
            }

            userId = Guid.NewGuid().ToString();

            context?.Response.Cookies.Append(AnonymousUser, userId);

            return userId;
        }


        public string GetAnonymousUserId()
        {
            var context = _httpContextAccessor.HttpContext;

            string? userId;

            if (context?.Request.Cookies.TryGetValue(AnonymousUser, out userId) == true && userId != null)
            {
                return userId;
            }

            userId = Guid.NewGuid().ToString();

            return userId;
        }

        public void ClearAnonymousUserCookie()
        {
            var context=  _httpContextAccessor.HttpContext;
          
            context?.Response.Cookies.Delete(AnonymousUser);
        }
    }
}
