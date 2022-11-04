using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace tothm_szak.ProcessMethods
{
    internal class cannyEdgeDetection
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);
       
                Cv2.GaussianBlur(processedImage, processedImage, new Size(3, 3), 0, 0, BorderTypes.Default);   
                Cv2.Canny(processedImage, processedImage, 70, 95, 3, true);
                return processedImage;
            } else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
    }
}
