

using Volo.Abp.Domain.Entities;

namespace MicroStore.ShoppingCart.Domain.Entities
{
    public class BasketItem : Entity<Guid>
    {

        public Product Product { get; private set; }
        public int Quantity { get; internal set; }

        protected BasketItem() { }
        public BasketItem(Product product, int quantity) : base(Guid.NewGuid())
        {
            Product = product;
            Quantity = quantity;
        }

    }
}
