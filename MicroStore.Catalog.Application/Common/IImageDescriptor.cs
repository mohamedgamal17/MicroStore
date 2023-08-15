using Emgu.CV;

namespace MicroStore.Catalog.Application.Common
{
    public interface IImageDescriptor
    {
        List<float> Descripe(Mat imageMat);
    }
}
