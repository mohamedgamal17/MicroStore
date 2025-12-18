using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Grpc.Billing;
namespace MicroStore.Bff.Shopping.Services.Billing
{
    public class PaymentSystemService
    {
        private readonly Grpc.Billing.PaymentSystemService.PaymentSystemServiceClient _paymentSystemServiceClient;

        public PaymentSystemService(Grpc.Billing.PaymentSystemService.PaymentSystemServiceClient paymentSystemServiceClient)
        {
            _paymentSystemServiceClient = paymentSystemServiceClient;
        }

        public async Task<List<PaymentSystem>> ListAsync(CancellationToken cancellationToken = default)
        {
            var request = new PaymentSystemListRequest();

            var result = await  _paymentSystemServiceClient.GetListAsync(request);

            return result.Items.Select(x => new PaymentSystem
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Image = x.Image,
                IsEnabled = x.IsEnabled
            }).ToList();
        }
    }
}
