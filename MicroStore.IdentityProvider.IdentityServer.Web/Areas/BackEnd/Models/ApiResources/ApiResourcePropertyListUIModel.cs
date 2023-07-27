﻿using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourcePropertyListUIModel : BaseListModel
    {
        public List<ApiResourcePropertyDto> Data { get; set; }
    }
}