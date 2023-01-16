using AutoMapper;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Domain;

namespace MicroStore.Payment.Application.Abstractions.Profiles
{
    public class PaymentProfile : Profile
    {

        public PaymentProfile()
        {
            CreateMap<PaymentRequest, PaymentRequestDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<PaymentRequest, PaymentRequestListDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.CreationTime, opt => opt.MapFrom(c => c.CreationTime));
                

            CreateMap<PaymentRequestProduct, PaymentRequestProductDto>();

        }
    }
}
