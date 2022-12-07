

using System.Drawing.Imaging;
using System.Drawing;

namespace MicroStore.Catalog.Application.Tests.Utilites
{
    public static class ImageGenerator
    {
        public static byte[] GetBitmapData()
        {
            Bitmap image = new Bitmap(50, 50);

            Graphics imageData = Graphics.FromImage(image);
            imageData.DrawLine(new Pen(Color.Red), 0, 0, 50, 50);

            MemoryStream memoryStream = new MemoryStream();
            byte[] bitmapData;

            using (memoryStream)
            {
                image.Save(memoryStream, ImageFormat.Png);
                bitmapData = memoryStream.ToArray();
            }
            return bitmapData;
        }
    }
}
