using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Billing
{
    public class PaymentRequestProfile : Profile
    {
        public PaymentRequestProfile()
        {
            CreateMap<PaymentRequest, PaymentRequestVM>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));
            CreateMap<PaymentRequestAggregate, PaymentRequestVM>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));
            CreateMap<PaymentRequestProduct, PaymentRequestProductVM>();

            CreateMap<PagedList<PaymentRequest>, PagedList<PaymentRequestVM>>();
        }
    }
}
