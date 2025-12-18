using AutoMapper;
using MicroStore.Payment.Domain.Shared.Configuration;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Application.Mappers
{
    public class PaymentSystemMapper : Profile
    {
        public PaymentSystemMapper()
        {
            CreateMap<PaymentSystem, PaymentSystemDto>();

        }
    }
}
