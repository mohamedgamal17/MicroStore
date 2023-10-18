#pragma warning disable CS8618

namespace MicroStore.Payment.Domain.Shared.Configuration
{
    public class PaymentSystem
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Image { get; private set; }

        public Type Provider { get; private set; }



        public PaymentProviderSetting Configuration
        {
            get
            {
                if (IsEnabled)
                {
                    return _configuration!;
                }

                throw new InvalidOperationException($"System {Name} must has defined configuration");
            }
        }

        public bool IsEnabled => _configuration != null;

        private readonly PaymentProviderSetting? _configuration;


        public PaymentSystem(string name, string disblayName, string image, Type provider, PaymentProviderSetting? configuration = null)
        {
            Name = name;
            DisplayName = disblayName;
            Image = image;
            Provider = provider;

            _configuration = configuration;

        }


        public PaymentSystem()
        {

        }

        public static PaymentSystem Create(string name, string disblayName, string image, Type provider, PaymentProviderSetting? configuration = null)
        {
            return new PaymentSystem(name, disblayName, image, provider, configuration);
        }
    }


    public class PaymentSystemOptions
    {
        public List<PaymentSystem> Systems { get; set; } = new List<PaymentSystem>();

    }
}
