using AutoMapper;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Application.Mappers
{
    public class PaymentSystemMapper : Profile
    {
        public PaymentSystemMapper()
        {
            CreateMap<PaymentSystem, PaymentSystemDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));

        }
    }
}
