using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class adaptiveThreshold : IClassification
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);

                processedImage = processedImage.MedianBlur(3);
                Cv2.AdaptiveThreshold(processedImage, processedImage, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 7, 7);
                return processedImage;
            } else
            {
                return (new Mat(256, 256, MatType.CV_8UC1, 0));
            }
        }
    }
}
