namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Inventory
{
    public class InventoryItemListModel : BasePagedListModel
    {

        public List<InventoryItemVM> Data { get; set; }
    }
}
