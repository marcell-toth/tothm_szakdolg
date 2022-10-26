using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class otsuThreshold
    {
        public Mat otsuThresholdImg(Mat src)
        {
            Cv2.MedianBlur(src, src, 3);
            Cv2.GaussianBlur(src, src, new Size(3,3), 3, 3,BorderTypes.Default);
            Cv2.Threshold(src, src, 150, 255, ThresholdTypes.Otsu);
            return src;
        }
    }
}
