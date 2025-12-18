using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class PaymentRequestAggregateService :
        IListableWithPaging<PaymentRequestAggregate, PaymentListRequestOptions>,
        IRetrievable<PaymentRequestAggregate>

    {
        private readonly PaymentRequestService _paymentRequestService;
        private readonly ProfileService _porfileService;
        public PaymentRequestAggregateService(PaymentRequestService paymentRequestService, ProfileService porfileService)
        {
            _paymentRequestService = paymentRequestService;
            _porfileService = porfileService;
        }

        public async Task<PagedList<PaymentRequestAggregate>> ListAsync(PaymentListRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await _paymentRequestService.ListAsync(options, requestHeaderOptions, cancellationToken);

            var tasks = response.Items.Select(x => PreparePaymentRequestAggregate(x, cancellationToken));

            var aggregates = await Task.WhenAll(tasks);

            var pagedModel = new PagedList<PaymentRequestAggregate>
            {
                Items = aggregates.ToList(),
                Skip = response.Skip,
                Lenght = response.Lenght,
                TotalCount = response.TotalCount
            };

            return pagedModel;
        }

        public async Task<PaymentRequestAggregate> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var paymentRequest = await _paymentRequestService.GetAsync(id, requestHeaderOptions, cancellationToken);


            return await PreparePaymentRequestAggregate(paymentRequest,cancellationToken);
        }

        private async Task<PaymentRequestAggregate> PreparePaymentRequestAggregate(PaymentRequest paymentRequest , CancellationToken cancellationToken)
        {
            var userProfile = await _porfileService.GetAsync(paymentRequest.UserId, cancellationToken: cancellationToken);

            var aggregate = new PaymentRequestAggregate
            {
                Id = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                UserId = paymentRequest.UserId,
                TransctionId = paymentRequest.TransctionId,
                PaymentGateway = paymentRequest.PaymentGateway,
                User = userProfile,
                TaxCost = paymentRequest.TaxCost,
                ShippingCost = paymentRequest.ShippingCost,
                SubTotal = paymentRequest.SubTotal,
                TotalCost = paymentRequest.TotalCost,
                Status = paymentRequest.Status,
                Items = paymentRequest.Items,
                CapturedAt = paymentRequest.CapturedAt,
                OpenedAt = paymentRequest.OpenedAt,
                Description = paymentRequest.Description,
                CreatedAt = paymentRequest.CreatedAt

            };

            return aggregate;
        }
    }
}
