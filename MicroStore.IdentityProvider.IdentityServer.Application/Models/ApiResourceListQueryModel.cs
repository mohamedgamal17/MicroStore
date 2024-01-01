using FluentValidation;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ApiResourceListQueryModel : PagingQueryParams
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }

}
