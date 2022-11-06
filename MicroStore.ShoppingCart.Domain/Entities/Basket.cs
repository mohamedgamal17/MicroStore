
using Volo.Abp.Domain.Entities;

namespace MicroStore.ShoppingCart.Domain.Entities
{
    public class Basket : BasicAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }

        private readonly IList<BasketItem> _lineItems = new List<BasketItem>();
        public IEnumerable<BasketItem> LineItems
        {
            get { return _lineItems.AsEnumerable(); }
        }

        public Basket(Guid userId)
            : base(Guid.NewGuid())

        {
            UserId = userId;
        }


        private Basket()
        {

        }


        public void AddItem(Product product, int quantity)
        {
            BasketItem? basketItem = FindBasketItem(product.Id);

            if (basketItem == null)
            {
                basketItem = new BasketItem(product, quantity);
                _lineItems.Add(basketItem);


            }
            else
            {
                basketItem.Quantity += quantity;
            }
        }


        public void UpdatetBasketItemQuantity(Guid basketItemId, int quantity)
        {

            if (!IsBasketItemExist(basketItemId))
            {
                throw new InvalidOperationException($"Basket item with id {basketItemId} is not exist");
            }

            BasketItem basketItem = _lineItems.Single(x => x.Id == basketItemId);

            basketItem.Quantity = quantity;

            RemoveEmptyItems();
        }





        public void RemoveItem(Product product)
        {
            RemoveItem(product.Id);
        }

        public void RemoveItem(Guid basketItemId)
        {
            if (!IsBasketItemExist(basketItemId))
            {
                throw new InvalidOperationException("Basket item is not exist");
            }

            BasketItem basketItem = _lineItems.Single(x => x.Id == basketItemId);

            _lineItems.Remove(basketItem);
        }

        public Result CanRemoveItem(Guid basketItemId)
        {
            BasketItem? basketItem = _lineItems.SingleOrDefault(x => x.Id == basketItemId);


            if (basketItem == null)
            {
                return Result.Failure($"Basket item id :{basketItemId} is not exist in current basket");
            }

            return Result.Success();
        }



        public decimal CalculateTotalPrice()
        {
            decimal price = 0;

            foreach (var item in _lineItems)
            {
                price += item.Quantity * item.Product.Price;
            }

            return price;
        }

        private BasketItem? FindBasketItem(Guid productId)
        {
            return _lineItems.SingleOrDefault(x => x.Product.Id == productId);
        }


        private void RemoveEmptyItems()
        {
            _lineItems.RemoveAll(x => x.Quantity == 0);
        }


        public bool IsBasketItemExist(Guid basketItemId) => _lineItems.Any(x => x.Id == basketItemId);

        public void Clear()
        {
            _lineItems.Clear();
        }

    }
}
