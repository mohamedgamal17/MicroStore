namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Inventory
{
    public class InventoryItemVM
    {
        public string  Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public int Stock { get; private set; }
        public int AllocatedStock { get; private set; }

    }
}
