using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
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
                UserName = userId,
                Items = new List<BasketItemAggregate>()
            };

            var basketResponse = await _basketService.RetrieveAsync(userId, cancellationToken: cancellationToken);


            if(basketResponse != null)
            {
                var tasks = basketResponse.Items
                    .Select(x => PreapreBasketItemAggregate(x, cancellationToken));

                var items =  await Task.WhenAll(tasks);

                basketAggregate.Items.AddRange(items);

            }
            return basketAggregate;
        }


        private async Task<BasketItemAggregate> PreapreBasketItemAggregate(BasketItem item, CancellationToken cancellationToken)
        {
            var product = await _productService.GetAsync(item.ProductId, cancellationToken: cancellationToken);

            var itemAggregate =  new BasketItemAggregate
            {
                ProductId = product.Id.ToString(),
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Name = product.Name,
                Sku = product.Sku,
                Thumbnail = product.ProductImages.FirstOrDefault()?.Image,
                Price = product.Price,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                Quantity = item.Quantity
            };


            return itemAggregate;
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
                Thumbnail= product.ProductImages.FirstOrDefault()?.Image,
                Price = product.Price,
                Weight = product.Weight,
                Dimensions = product.Dimensions
            };
        }
    }
}
