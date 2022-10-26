using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class simpleThreshold
    {
        public Mat simpleThresholdImg(Mat src)
        {
            Cv2.Threshold(src, src, 150, 255, ThresholdTypes.Binary);
            return src;
        }
    }
}
