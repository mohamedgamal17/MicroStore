using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Payment.Application.PaymentSystems;

namespace MicroStore.Payment.Api.Host.Grpc
{
    public class PaymentSystemGrpcService : PaymentSystemService.PaymentSystemServiceBase
    {
        private readonly IPaymentSystemQueryService _paymentSystemQueryService;

        public PaymentSystemGrpcService(IPaymentSystemQueryService paymentSystemQueryService)
        {
            _paymentSystemQueryService = paymentSystemQueryService;
        }


        public override async Task<PaymentSystemListResponse> GetList(PaymentSystemListRequest request, ServerCallContext context)
        {
            var result = await _paymentSystemQueryService.ListAsync();

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            var response = new PaymentSystemListResponse();

            var items = result.Value.Select(x => new PaymentSystemResponse
            {
                Name = x.Name,
                Image = x.Image,
                DisplayName = x.DisplayName,
                IsEnabled = x.IsEnabled
            }).ToList();


            response.Items.AddRange(items);


            return response;
        }
    }
}
