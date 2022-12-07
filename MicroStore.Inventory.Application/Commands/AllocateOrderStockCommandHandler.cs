using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Commands
{
    public class AllocateOrderStockCommandHandler : CommandHandler<AllocateOrderStockCommand, Result>
    {

        private readonly IRepository<Product> _productRepository;

        public AllocateOrderStockCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<Result> Handle(AllocateOrderStockCommand request, CancellationToken cancellationToken)
        {
            List<Result> failureResults = new List<Result>();

            foreach (var orderItem in request.Products)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.Id == orderItem.ProductId);

                var result = product.CanAllocateQuantity(orderItem.Quantity);

                if (result.IsFailure)
                {
                    failureResults.Add(result);
                }
            }

            if (failureResults.Any())
            {

                string details = failureResults.Select(x => x.ToString())
                        .Aggregate((t1, t2) => t1 + "\n" + t2)!;

                return Result.Failure(details);
            }


            foreach (var orderItem in request.Products)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.Id == orderItem.ProductId);

                product.AllocateStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);
            }

            return Result.Success();
        }
    }
}
