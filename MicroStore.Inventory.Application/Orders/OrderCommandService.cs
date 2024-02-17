using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.Exceptions;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace MicroStore.Inventory.Application.Orders
{
    [DisableValidation]
    public class OrderCommandService :InventoryApplicationService ,IOrderCommandService
    {
        private readonly IRepository<Product> _productRepository;


        public OrderCommandService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Unit>> AllocateOrderStockAsync(OrderStockModel model, CancellationToken cancellationToken = default)
        {
            List<Result<Unit>> failureResults = new();


            Dictionary<string, string> errors = new Dictionary<string, string>();

            foreach (var orderItem in model.Items)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id
                == orderItem.ProductId, cancellationToken);

                var result = product.CanAllocateStock(orderItem.Quantity);

                if (result.IsFailure)
                {
                    errors[orderItem.ProductId] = result.Exception.Message; 
                }
            }

            if (errors.Any())
            {
                return   new Result<Unit>(new OrderStockException(errors));           
            }

            foreach (var orderItem in model.Items)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.Id == orderItem.ProductId,cancellationToken);

                product.AllocateStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);
            }

            return Unit.Value;
        }

        public async Task<Result<Unit>> ReleaseOrderStockAsync(OrderStockModel model, CancellationToken cancellationToken = default)
        {

            foreach (var orderItem in model.Items)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id == orderItem.ProductId, cancellationToken);

                product.ReleaseStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product,cancellationToken: cancellationToken);
            }

            return Unit.Value;
        }
    }


}
