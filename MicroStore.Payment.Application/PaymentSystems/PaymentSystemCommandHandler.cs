using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using System.Net;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Payment.Application.PaymentSystems
{
    internal class PaymentSystemCommandHandler : RequestHandler,
        ICommandHandler<UpdatePaymentSystemCommand, PaymentSystemDto>
    {
        private readonly IRepository<PaymentSystem> _paymentSystemRepository;

        public PaymentSystemCommandHandler(IRepository<PaymentSystem> paymentSystemRepository)
        {
            _paymentSystemRepository = paymentSystemRepository;
        }
        public async Task<ResponseResult<PaymentSystemDto>> Handle(UpdatePaymentSystemCommand request, CancellationToken cancellationToken)
        {
            PaymentSystem? paymentSystem = await _paymentSystemRepository.SingleOrDefaultAsync(x => x.Name == request.Name);

            if (paymentSystem == null)
            {
                return Failure<PaymentSystemDto>(HttpStatusCode.NotFound, new ErrorInfo { Message = $"Payment system with name :{request.Name}, is not exist" });
            }

            paymentSystem.IsEnabled = request.IsEnabled;

            await _paymentSystemRepository.UpdateAsync(paymentSystem, cancellationToken: cancellationToken);



            return Success(HttpStatusCode.OK, ObjectMapper.Map<PaymentSystem, PaymentSystemDto>(paymentSystem));
        }
    }
}
