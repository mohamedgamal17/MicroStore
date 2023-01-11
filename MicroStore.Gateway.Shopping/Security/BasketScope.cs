namespace MicroStore.Gateway.Shopping.Security
{
    public static class BasketScope
    {

        public const string Read = "basket.read";

        public const string Update = "basket.update";

        public const string Migrate = "basket.migrate";


        public static List<string> List()
        {
            return new List<string>()
            {
                Read,
                Update,
                Migrate
            };
        }
    }
}
