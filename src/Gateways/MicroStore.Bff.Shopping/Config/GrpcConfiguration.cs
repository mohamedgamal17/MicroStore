namespace MicroStore.Bff.Shopping.Config
{
    public class GrpcConfiguration : Dictionary<string , string>
    {
        public string Catalog => FindByKey(nameof(Catalog));
        public string Basket => FindByKey(nameof(Basket));
        public string Ordering => FindByKey(nameof(Ordering));
        public string Billing => FindByKey(nameof(Billing));
        public string Shipping => FindByKey(nameof(Shipping));
        public string Profiling => FindByKey(nameof(Profiling));
        public string Geographic => FindByKey(nameof(Geographic));
        public string Inventory => FindByKey(nameof(Inventory));
        private string FindByKey(string key)
        {
            if(TryGetValue(key,  out var value))
            {
                return value;
            }

            return string.Empty;
        }



    }



}
