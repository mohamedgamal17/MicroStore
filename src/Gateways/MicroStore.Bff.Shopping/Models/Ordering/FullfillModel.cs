namespace MicroStore.Bff.Shopping.Models.Ordering
{
    public class FullfillModel
    {
        public string ShipmentId { get; set; }
        public FullfillModel()
        {
            ShipmentId = string.Empty;    
        }
    }
}
