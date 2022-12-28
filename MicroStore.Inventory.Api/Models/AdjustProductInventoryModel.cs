namespace MicroStore.Inventory.Api.Models
{
    public class AdjustProductInventoryModel
    {
        public int AdjustedQuantity { get; set; }

        public string Reason { get; set; }
    }
}
