using AutoMapper;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Domain;

namespace MicroStore.Payment.Application.Abstractions.Profiles
{
    public class PaymentProfile : Profile
    {

        public PaymentProfile()
        {
            CreateMap<PaymentRequest, PaymentRequestCreatedDto>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<PaymentRequestProduct, PaymentRequestProductDto>();

            CreateMap<PaymentRequest, PaymentRequestCompletedDto>();
        }
    }
}
