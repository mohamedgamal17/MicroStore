using AutoMapper;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared.Mappers
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
