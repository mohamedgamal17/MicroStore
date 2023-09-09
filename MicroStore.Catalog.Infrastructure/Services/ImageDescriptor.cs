using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using MicroStore.Catalog.Domain.Entities;
using System.Drawing;
using System.Collections;
using MicroStore.Catalog.Application.Common;
using Microsoft.Extensions.Options;

namespace MicroStore.Catalog.Infrastructure.Services
{
    public abstract class ImageDescriptor : IImageDescriptor
    {
        public ImageDescriptorOptions Options { get; protected set; }

        protected ImageDescriptor(IOptions<ImageDescriptorOptions> options)
        {
            Options = options.Value;
        }

        public abstract int[] Channels { get; protected set; }
        public abstract float[] Ranges { get; protected set; }


        public List<float> Descripe(Mat imageMat)
        {
            var ellispedMask = DrawEllispedMask(imageMat.Width, imageMat.Height);

            var segments = GetSegments(imageMat.Width, imageMat.Height);

            List<float> features = new List<float>();

            features.AddRange(CalcHist(imageMat, ellispedMask));

            foreach (var segment in segments)
            {
                var cornerMask = DrawCornerMask(imageMat.Width, imageMat.Height, segment);

                CvInvoke.Subtract(cornerMask, ellispedMask, cornerMask);

                features.AddRange(CalcHist(imageMat, cornerMask));
            }


            return features;
        }


        protected (int, int) GetCenters(int width, int height)
        {
            return (width / 2, height / 2);
        }

        protected Mat DrawEllispedMask(int width, int height)
        {
            var (cx, cy) = GetCenters(width, height);

            var blank = new Image<Gray, byte>(width, height);


            int areax = Convert.ToInt32((blank.Width * 0.75) / 2);

            int areay = Convert.ToInt32((blank.Height * 0.75) / 2);

            var ellipsePoint = new Point(cx, cy);

            var ellipseSize = new Size(areax, areay);

            CvInvoke.Ellipse(blank, ellipsePoint, ellipseSize, 0, 0, 360, new MCvScalar(256), -1);

            return blank.Mat;
        }


        protected Mat DrawCornerMask(int width, int height, Segment segment)
        {
            var blank = new Image<Gray, byte>(width, height);

            var rectangle = new Rectangle(new Point(segment.StartX, segment.StartY), new Size(segment.EndX, segment.EndY));

            CvInvoke.Rectangle(blank, rectangle, new MCvScalar(255), -1);

            return blank.Mat;
        }
        protected List<Segment> GetSegments(int width, int height)
        {
            var (cx, cy) = GetCenters(width, height);

            List<Segment> segments = new List<Segment>
            {
                new Segment( 0,cx , 0, cy),
                new Segment( cx, width , 0, cy ),
                new Segment(cx, width, cy, height ),
                new Segment(0,cx, cy, height ),
            };

            return segments;
        }



        protected float[] CalcHist(Mat image, Mat mask)
        {
            var vectorOfMat = new VectorOfMat(image);

            Mat hist = new Mat();

            CvInvoke.CalcHist(vectorOfMat, Channels, mask, hist, Options.Bins, Ranges, true);

            CvInvoke.Normalize(hist, hist);

            float[] data = Flatten<float>(hist.GetData());

            return data;
        }

        protected T[] Flatten<T>(Array data)
        {
            var list = new List<T>();
            var stack = new Stack<IEnumerator>();
            stack.Push(data.GetEnumerator());
            do
            {
                for (var iterator = stack.Pop(); iterator.MoveNext();)
                {
                    if (iterator.Current is Array)
                    {
                        stack.Push(iterator);
                        iterator = (iterator.Current as IEnumerable).GetEnumerator();
                    }
                    else
                        list.Add((T)iterator.Current);
                }
            }
            while (stack.Count > 0);
            return list.ToArray();
        }
    }
}
