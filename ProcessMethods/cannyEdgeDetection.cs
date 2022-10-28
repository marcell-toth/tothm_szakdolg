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
        public Mat cannyEdgeDetectionImg(Mat src)
        {
            Mat dst = new();
            Cv2.GaussianBlur(src, src, new Size(3,3), 0, 0, BorderTypes.Default);
            Cv2.Canny(src, dst, 70, 95, 3, true);
            return dst;
        }
    }
}
