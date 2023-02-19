using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Cart
{
    public class BasketAggregateService
    {
        private readonly BasketService _basketService;

        private readonly ProductService _productService;

        public BasketAggregateService(BasketService basketService, 
            ProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }

        public async Task<BasketAggregate> RetriveBasket(string userId, CancellationToken cancellationToken = default)
        {
            var basketAggregate = new BasketAggregate
            {
                UserId = userId,
                Items = new List<BasketItemAggregate>()
            };

            var basketResponse = await _basketService.RetrieveAsync(userId, cancellationToken);

            basketResponse.ThrowIfFailure();

            foreach (var item in basketResponse.Result.Items)
            {
                var productResponse =  await _productService.RetriveAsync(Guid.Parse(item.ProductId), cancellationToken);

                productResponse.ThrowIfFailure();

                var basketItem = PrepareBasketItemAggregate(productResponse.Result);

                basketItem.Quantity = item.Quantity;

                basketAggregate.Items.Add(basketItem);
            }


            return basketAggregate;
        }



        private BasketItemAggregate PrepareBasketItemAggregate(Product product)
        {
            return new BasketItemAggregate
            {
                ProductId = product.Id.ToString(),
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Name = product.Name,
                Sku = product.Sku,
                Thumbnail= product.Thumbnail,
                Price = product.Price,
                Weight = product.Weight,
                Dimensions = product.Dimensions
            };
        }
    }
}
