using AutoMapper;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Application.Mappers
{
    public class PaymentMapper : Profile
    {

        public PaymentMapper()
        {
            CreateMap<PaymentRequest, PaymentRequestDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
                .ForMember(x => x.Status, opt => opt.MapFrom(c => c.State));


            CreateMap<PaymentRequestProduct, PaymentRequestProductDto>();

        }
    }
}
