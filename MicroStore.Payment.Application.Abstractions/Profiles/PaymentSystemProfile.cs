using AutoMapper;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Domain;
namespace MicroStore.Payment.Application.Abstractions.Profiles
{
    public class PaymentSystemProfile : Profile
    {
        public PaymentSystemProfile()
        {
            CreateMap<PaymentSystem, PaymentSystemDto>()
                .ForMember(x => x.SystemId, opt => opt.MapFrom(c => c.Id));

        }
    }
}
