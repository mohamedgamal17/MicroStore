using Microsoft.Extensions.Logging;
namespace MicroStore.Shipping.PluginInMemoryTest.Utilites
{

    public class TestOutputLoggerFactory :
            ILoggerFactory
    {
        readonly bool _enabled;

        public TestOutputLoggerFactory(bool enabled)
        {
            _enabled = enabled;
        }

        public NUnit.Framework.Internal.TestExecutionContext Current { get; set; }

        public ILogger CreateLogger(string name)
        {
            return new TestOutputLogger(this, _enabled);
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
        }
    }
}
