using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Shipping.Application.Commands
{
    public class UpdateShippingSystemCommandHandler : CommandHandler<UpdateShippingSystemCommand>
    {
        private readonly IRepository<ShippingSystem> _shippingSystemRepository;

        public UpdateShippingSystemCommandHandler(IRepository<ShippingSystem> shippingSystemRepository)
        {
            _shippingSystemRepository = shippingSystemRepository;
        }

        public override async Task<ResponseResult> Handle(UpdateShippingSystemCommand request, CancellationToken cancellationToken)
        {
            var system = await _shippingSystemRepository.SingleOrDefaultAsync(x => x.Name == request.SystemName);

            if(system == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Shipping system with name : {request.SystemName} is not exist"
                });
            }


            system.IsEnabled = request.IsEnabled;

            await _shippingSystemRepository.UpdateAsync(system);


            return Success(HttpStatusCode.OK);
        }
    }
}
