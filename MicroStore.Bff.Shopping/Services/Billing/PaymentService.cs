using Google.Protobuf.WellKnownTypes;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Grpc.Billing;
using MicroStore.Bff.Shopping.Models.Billing;
namespace MicroStore.Bff.Shopping.Services.Billing
{
    public class PaymentService
    {
        private readonly Grpc.Billing.PaymentService.PaymentServiceClient _paymentService;

        public PaymentService(Grpc.Billing.PaymentService.PaymentServiceClient paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<PagedList<Payment>> ListAsync(string? userId = null,string? orderNumber = null, string? status = null, double minPrice = -1, double maxPrice = -1, DateTime? startDate = null , DateTime? endDate = null , int skip = 0, int length = 10 , string? sortBy = null , bool desc = false ,CancellationToken cancellationToken = default)
        {
            var request = new PaymentListRequest
            {
                UserId = userId,
                OrderNumber = orderNumber,
                Status = status,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                StartDate = startDate?.ToUniversalTime().ToTimestamp(),
                EndDate = endDate?.ToUniversalTime().ToTimestamp(),
                Skip = skip,
                Length = length,
                SortBy = sortBy,
                Desc = desc,
               
            };

            var response = await _paymentService.GetListAsync(request);

            var pagedList = new PagedList<Payment>
            {
                Skip = response.Skip,
                Length = response.Length,
                TotalCount = response.TotalCount,
                Items = response.Items.Select(PreparePayment).ToList()
            };

            return pagedList;
        }

        public async Task<List<Payment>> ListByOrderIdsAsync(List<string> orderIds, CancellationToken cancellationToken = default)
        {
            var request = new PaymentListByOrderIdsRequest();

            orderIds?.ForEach(id => request.OrderIds.Add(request.OrderIds));

            var response = await _paymentService.GetListByOrderIdsAsync(request);

            return response.Items.Select(PreparePayment).ToList();
        }


        public async Task<List<Payment>> ListByOrderNumbersAsync(List<string> orderNumbers ,CancellationToken cancellationToken = default)
        {
            var request = new PaymentListByOrderNumbersRequest();

            request.OrderNumbers.AddRange(orderNumbers);

            var response = await _paymentService.GetListByOrderNumbersAsync(request);

            return response.Items.Select(PreparePayment).ToList();
        }
        public async Task<Payment> GetAsync(string paymentId , CancellationToken cancellationToken = default)
        {
            var request = new GetPaymentByIdReqeust { Id = paymentId };

            var response = await _paymentService.GetByIdAsync(request);

            return PreparePayment(response);
        }

        public async Task<Payment> GetByOrderIdAsync(string orderId,  CancellationToken cancellationToken = default)
        {
            var request = new GetPaymentByOrderIdRequest { OrderId = orderId };

            var response = await _paymentService.GetByOrderIdAsync(request);

            return PreparePayment(response);
        }

        public async Task<Payment> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var request = new GetPaymentByOrderNumberRequest { OrderNumber = orderNumber };

            var response = await _paymentService.GetByOrderNumberAsync(request);

            return PreparePayment(response);
        }
        public async Task<Payment> CreateAsync(CreatePaymentModel model, CancellationToken cancellationToken = default)
        {
            var request = PrepareCreatePaymentRequest(model);

            var response = await _paymentService.CreateAsync(request);

            return PreparePayment(response);
        }

        public async Task<PaymentProcess> ProcessAsync(string paymentId, ProcessPaymentModel model , CancellationToken cancellationToken = default)
        {
            var request = new ProcessPaymentReqeust
            {
                Id = paymentId,
                GatewayName = model.GatewayName,
                ReturnUrl = model.ReturnUrl,
                CancelUrl = model.CancelUrl
            };

            var response = await _paymentService.ProcessAsync(request);

            return new PaymentProcess
            {
                SessionId = response.SessionId,
                TransactionId = response.TransactionId,
                AmountSubTotal = response.AmountSubTotal,
                AmountTotal = response.AmountTotal,
                SuccessUrl = response.SuccessUrl,
                CancelUrl = response.CancelUrl,
                CheckoutLink = response.CheckoutLink,
                Provider = response.Provider

            };
        }

        public async Task<Payment> CompleteAsync(CompletePaymentModel model, CancellationToken cancellationToken = default)
        {
            var request = new CompletePaymentRequest
            {
                SessionId = model.SessionId,
                GatewayName = model.GatewayName
            };

            var response = await _paymentService.CompleteAsync(request);

            return PreparePayment(response);
        }

        public async Task<Payment> RefundAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            var request = new RefundPaymentRequest { PaymentRequestId = paymentId };

            var response = await _paymentService.RefundAsync(request);

            return PreparePayment(response);
        }


        private CreatePaymentRequest PrepareCreatePaymentRequest(CreatePaymentModel model)
        {
            var request = new CreatePaymentRequest
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNumber,
                UserId = model.UserId,
                ShippingCost = model.ShippingCost,
                TaxCost = model.TaxCost,
                SubTotal = model.SubTotal,
                TotalCost = model.TotalCost,
                Description = model.Description,
            };

            if(model.Items != null)
            {
                foreach (var item in model.Items)
                {
                    var paymentItem = new PaymentProductRequest
                    {
                        Name = item.Name,
                        Sku = item.Sku,
                        ProductId = item.ProductId,
                        Thumbnail = item.Image,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    };
                    request.Items.Add(paymentItem);
                }
            }

            return request;
        }

        private Payment PreparePayment(PaymentResponse response)
        {
            var payment = new Payment
            {
                Id = response.Id,
                OrderId = response.OrderId,
                OrderNumber = response.OrderNumber,
                UserId = response.UserId,
                Status = (Data.Billing.PaymentStatus)response.Status,
                TaxCost = response.TaxCost,
                ShippingCost = response.ShippingCost,
                SubTotal = response.SubTotal,
                TotalCost = response.TotalCost,
                TransctionId = response.TransctionId,
                PaymentGateway = response.PaymentGateway,
                Description = response.Description,
                CapturedAt = response.CapturedAt?.ToDateTime(),
                RefundedAt = response.RefundedAt?.ToDateTime(),
                FaultAt = response.FaultAt?.ToDateTime(),
                Items = response.Items.Select(x => new PaymentItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sku = x.Sku,
                    ProductId = x.ProductId,
                    Thumbnail = x.Thumbnail,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList(),
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };
            return payment;
        }
    }
}
