using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class laplacian : IClassification
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);

                Cv2.Laplacian(processedImage, processedImage, MatType.CV_8UC1, 3, 1, 0, BorderTypes.Default);
                return processedImage;
            } else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
    }
}
