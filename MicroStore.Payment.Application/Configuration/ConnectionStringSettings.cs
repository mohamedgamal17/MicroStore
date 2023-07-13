namespace MicroStore.Payment.Application.Configuration
{
    public class ConnectionStringSettings : Dictionary<string, string>
    {
        public string DefaultConnection => FindByKey(nameof(DefaultConnection));

        private string FindByKey(string key)
        {
            if (TryGetValue(key, out var value))
            {
                return value;
            }

            return string.Empty;
        }
    }
}
