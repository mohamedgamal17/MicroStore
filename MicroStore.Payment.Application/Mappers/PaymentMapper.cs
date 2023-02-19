using AutoMapper;
using MicroStore.Payment.Domain;
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
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.State));

            CreateMap<PaymentRequest, PaymentRequestListDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.State))
                .ForMember(x => x.CreationTime, opt => opt.MapFrom(c => c.CreationTime));


            CreateMap<PaymentRequestProduct, PaymentRequestProductDto>();

        }
    }
}
