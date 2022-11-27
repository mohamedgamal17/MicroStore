using AutoMapper;
using MicroStore.Payment.Domain.Shared.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Application.Dtos
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
