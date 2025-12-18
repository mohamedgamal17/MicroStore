using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShipmentRateDto : EntityDto<string>
    {
        public string CarrierId { get; set; }
        public MoneyDto Amount { get; set; }
        public ServiceLevelDto ServiceLevel { get; set; }
        public int?  Days { get; set; }

    }
}
    