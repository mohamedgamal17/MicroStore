using MicroStore.Catalog.Application.Common;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Catalog.Infrastructure.Services
{
    [ExposeServices(typeof(IImageDescriptor), IncludeDefaults = true, IncludeSelf = true)]
    public class HsvImageDescriptor : ImageDescriptor
    {
        public override int[] Channels { get; protected set; }
        public override float[] Ranges { get; protected set; }

        public HsvImageDescriptor(ImageDescriptorOptions options) : base(options)
        {

            Channels = new int[] { 0, 1, 2 };

            Ranges = new float[] { 0, 256, 0, 256, 0, 256 };
        }

    }
}
