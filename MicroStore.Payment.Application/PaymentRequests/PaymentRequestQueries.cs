
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Payment.Domain.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.PaymentRequests
{
    public class GetPaymentRequestListQuery : PagingAndSortingQueryParams, IQuery<PagedResult<PaymentRequestListDto>>
    {
    }

    public class GetPaymentRequestQuery : IQuery<PaymentRequestDto>
    {
        public Guid PaymentRequestId { get; set; }

    }

    public class GetPaymentRequestWithOrderIdQuery : IQuery<PaymentRequestDto>
    {
        public string OrderId { get; set; }
    }
  
    public class GetUserPaymentRequestListQuery : PagingAndSortingQueryParams, IQuery<PagedResult<PaymentRequestListDto>>
    {
        public string UserId { get; set; }
    }
}
