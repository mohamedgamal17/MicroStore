namespace MicroStore.Inventory.Application.Abstractions.Dtos
{
    public class ProductRecivedDto
    {
        public Guid ProductId { get; set; }
        public int RecivedStock { get; set; }
        public int Stock { get; set; }
    }
}
