using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace MicroStore.Inventory.Application.Orders
{
    public class OrderCommandService :InventoryApplicationService ,IOrderCommandService
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Order> _orderRepository;

        public OrderCommandService(IRepository<Product> productRepository, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        [DisableValidation]
        public async Task<Result<OrderDto>> AllocateOrderStockAsync(AllocateOrderStockModel model, CancellationToken cancellationToken = default)
        {
            List<Result<Unit>> failureResults = new();

            foreach (var orderItem in model.Items)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id
                == orderItem.ProductId, cancellationToken);

                var result = product.CanAllocateStock(orderItem.Quantity);

                if (result.IsFailure)
                {
                    failureResults.Add(result);
                }
            }

            if (failureResults.Any())
            {

                string details = failureResults.Select(x => x.ToString())
                        .Aggregate((t1, t2) => t1 + "\n" + t2)!;



                return   new Result<OrderDto>(new BusinessException(details));
                
            }

            foreach (var orderItem in model.Items)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.Id == orderItem.ProductId,cancellationToken);

                product.AllocateStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);
            }

            Order order = new Order(model.OrderId)
            {

                OrderNumber = model.OrderNumber,
                PaymentId = model.PaymentId,
                UserId = model.UserId,
                ShippingAddress = model.ShippingAddress.AsAddressValueObject(),
                BillingAddres = model.ShippingAddress.AsAddressValueObject(),
                ShippingCost = model.ShippingCost,
                TaxCost = model.TaxCost,
                SubTotal = model.SubTotal,
                TotalPrice = model.TotalPrice,
                Items = model.Items.Select(x => new OrderItem(x.ItemId)
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Sku = x.Sku,
                    Thumbnail = x.Thumbnail,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,

                }).ToList()
            };

            await _orderRepository.InsertAsync(order,cancellationToken: cancellationToken);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [DisableValidation]
        public async Task<Result<OrderDto>> ReleaseOrderStockAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = await _orderRepository.WithDetailsAsync(x => x.Items);

            var order = await query.SingleAsync(x => x.Id == orderId,cancellationToken);

            foreach (var orderItem in order.Items)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id == orderItem.ProductId, cancellationToken);

                product.ReleaseStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product,cancellationToken: cancellationToken);
            }

            order.IsCancelled = true;

            await _orderRepository.UpdateAsync(order,cancellationToken:cancellationToken);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }


}
