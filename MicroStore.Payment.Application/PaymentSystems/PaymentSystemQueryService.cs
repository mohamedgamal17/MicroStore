using Microsoft.Extensions.Options;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Domain.Shared.Configuration;
using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Application.PaymentSystems
{
    public class PaymentSystemQueryService : PaymentApplicationService, IPaymentSystemQueryService
    {
        private readonly List<PaymentSystem> _paymentSystems;

        public PaymentSystemQueryService(IOptions<PaymentSystemOptions> options)
        {
            _paymentSystems = options.Value.Systems;
        }

        public Task<Result<List<PaymentSystemDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var systems = _paymentSystems.Select(x => new PaymentSystemDto
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Image = x.Image,
                IsEnabled = x.IsEnabled
            }).ToList();

            return Task.FromResult(new Result<List<PaymentSystemDto>>(systems));
        }
    }
}
