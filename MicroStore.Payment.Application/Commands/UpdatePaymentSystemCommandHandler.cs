using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Domain;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Application.Commands
{
    public class UpdatePaymentSystemCommandHandler : CommandHandler<UpdatePaymentSystemCommand>
    {
        private readonly IRepository<PaymentSystem> _paymentSystemRepository;

        public UpdatePaymentSystemCommandHandler(IRepository<PaymentSystem> paymentSystemRepository)
        {
            _paymentSystemRepository = paymentSystemRepository;
        }

        public override async Task<ResponseResult> Handle(UpdatePaymentSystemCommand request, CancellationToken cancellationToken)
        {
            PaymentSystem? paymentSystem = await _paymentSystemRepository.SingleOrDefaultAsync(x => x.Name == request.Name);

            if(paymentSystem == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo { Message = $"Payment system with name :{request.Name}, is not exist" });
            }

            paymentSystem.IsEnabled = request.IsEnabled;

            await _paymentSystemRepository.UpdateAsync(paymentSystem,cancellationToken : cancellationToken);



            return ResponseResult.Success((int) HttpStatusCode.Accepted ,ObjectMapper.Map<PaymentSystem, PaymentSystemDto>(paymentSystem));
        }
    }
}
