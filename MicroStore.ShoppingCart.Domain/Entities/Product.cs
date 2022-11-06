using Volo.Abp.Domain.Entities;

namespace MicroStore.ShoppingCart.Domain.Entities
{
    public class Product : Entity<Guid>
    {

        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }

        public Product(Guid id, string name, string sku, decimal price) : base(id)
        {
            Name = name;
            Sku = sku;
            Price = price;
        }

    }
}
