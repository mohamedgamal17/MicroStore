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
                .ForMember(x => x.PaymentId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<PaymentRequest, PaymentRequestListDto>()
                .ForMember(x => x.PaymentId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(c => c.CreationTime));
                

            CreateMap<PaymentRequest, PaymentRequestCreatedDto>()
                .ForMember(x=> x.PaymentId, opt=>opt.MapFrom(c=> c.Id))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(c => c.CreationTime));

            CreateMap<PaymentRequestProduct, PaymentRequestProductDto>();

            CreateMap<PaymentRequest, PaymentRequestCompletedDto>()
                .ForMember(x => x.PaymentId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(c => c.CreationTime));


        }
    }
}
