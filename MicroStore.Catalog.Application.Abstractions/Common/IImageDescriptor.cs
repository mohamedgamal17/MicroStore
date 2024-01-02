using Emgu.CV;

namespace MicroStore.Catalog.Application.Abstractions.Common
{
    public interface IImageDescriptor
    {
        List<float> Descripe(Mat imageMat);
    }
}
