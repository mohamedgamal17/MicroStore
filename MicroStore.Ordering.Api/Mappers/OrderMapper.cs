using AutoMapper;
using MicroStore.Ordering.Api.Models;
using MicroStore.Ordering.Application.Orders;

namespace MicroStore.Ordering.Api.Mappers
{
    public class OrderMapper : Profile
    {

        public OrderMapper()
        {
            CreateMap<OrderModel, SubmitOrderCommand>();

            CreateMap<CreateOrderModel, SubmitOrderCommand>();

        }
    }
}
