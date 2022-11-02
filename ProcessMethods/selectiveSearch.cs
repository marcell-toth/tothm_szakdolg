using OpenCvSharp.XImgProc.Segmentation;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class selectiveSearch
    {
        public Mat searchSegmentImg(Mat src)
        {
            var ss = SelectiveSearchSegmentation.Create();

            Cv2.GaussianBlur(src, src, new Size(3, 3), 0, 0, BorderTypes.Default);
            Cv2.Threshold(src, src, 150, 255, ThresholdTypes.Otsu);
            Cv2.CvtColor(src, src, ColorConversionCodes.GRAY2BGR);

            ss.SetBaseImage(src);
            ss.SwitchToSingleStrategy(350, 0.95F);

            //ss.SwitchToSelectiveSearchFast(50, 200, 0.8F);
            //List<OpenCvSharp.Rect> rl = new List<OpenCvSharp.Rect>();
            //rl.ToArray();

            OpenCvSharp.Rect[] rl;
            ss.Process(out rl);
            Trace.WriteLine(rl.Length);

            Scalar sc = new Scalar(0, 0, 255);
            foreach (OpenCvSharp.Rect r in rl)
            {
                Cv2.Rectangle(src, r.TopLeft, r.BottomRight, sc, 1, LineTypes.Link4, 0);
            }
            return src;
        }
    }
}
