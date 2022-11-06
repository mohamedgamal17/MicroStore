namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
