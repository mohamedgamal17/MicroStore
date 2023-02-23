using AutoMapper;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Mappers
{
    public class ApplicationIdentityClaimProfile : Profile
    {
        public ApplicationIdentityClaimProfile()
        {
            CreateMap<ApplicationIdentityUserClaim, IdentityClaimDto>()
                .ForMember(x => x.ClaimValue, opt => opt.MapFrom(c => c.ClaimValue))
                .ForMember(x => x.ClaimType, opt => opt.MapFrom(c => c.ClaimType))
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));



            CreateMap<ApplicationIdentityRoleClaim, IdentityClaimDto>()
                .ForMember(x => x.ClaimValue, opt => opt.MapFrom(c => c.ClaimValue))
                .ForMember(x => x.ClaimType, opt => opt.MapFrom(c => c.ClaimType))
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));

        }
    }
}
