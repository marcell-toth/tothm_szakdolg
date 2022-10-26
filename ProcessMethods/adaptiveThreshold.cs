using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class adaptiveThreshold
    {
        public Mat adaptiveThresholdImg(Mat src)
        {
            Mat dst = src.Clone();
            dst = dst.MedianBlur(3);
            Cv2.AdaptiveThreshold(dst, dst, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 7, 7);
            return dst;
        }
    }
}
