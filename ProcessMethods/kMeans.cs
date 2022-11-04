using OpenCvSharp;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class kMeans : IClassification
    {
        public Mat? baseImage { get; set; }

        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2HSV);
                processedImage.ConvertTo(processedImage, MatType.CV_32FC1);

                //Mat labels = new Mat();
                //Mat centers = new Mat();
                //Cv2.Kmeans(processedImage, 4, labels, TermCriteria.Both(40, 40), 1, KMeansFlags.PpCenters, centers);

                processedImage.ConvertTo(processedImage, MatType.CV_8UC3);
                Cv2.CvtColor(processedImage, processedImage, ColorConversionCodes.HSV2BGR);

                return processedImage;
            }
            else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
    }
}
