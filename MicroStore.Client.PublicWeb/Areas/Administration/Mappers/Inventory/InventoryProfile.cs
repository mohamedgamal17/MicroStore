using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Inventory;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Inventory
{
    public class InventoryProfile : Profile
    {

        public InventoryProfile()
        {
            CreateMap<InventoryItem, InventoryItemVM>();

            CreateMap<InventoryItemModel, InventoryItemRequestOptions>();
        }
    }
}
