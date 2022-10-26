using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class laplacian
    {
        public Mat gradLaplacianImg(Mat src)
        {
            Cv2.Laplacian(src, src, MatType.CV_8UC3, 3, 1, 0, BorderTypes.Default);
            return src;
        }
    }
}
