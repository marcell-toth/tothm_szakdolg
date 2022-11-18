using OpenCvSharp;
using OpenCvSharp.XImgProc.Segmentation;
using System.Diagnostics;

namespace tothm_szak.ProcessMethods
{
    internal class selectiveSearch : IClassification
    {
        /// <summary>
        /// Selective search method
        /// Unfinished
        /// </summary>
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                var ss = SelectiveSearchSegmentation.Create();

                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);
                Cv2.GaussianBlur(processedImage, processedImage, new Size(3, 3), 0, 0, BorderTypes.Default);
                Cv2.Threshold(processedImage, processedImage, 150, 255, ThresholdTypes.Otsu);
                Cv2.CvtColor(processedImage, processedImage, ColorConversionCodes.GRAY2BGR);

                ss.SetBaseImage(processedImage);
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
                    Cv2.Rectangle(baseImage, r.TopLeft, r.BottomRight, sc, 1, LineTypes.Link4, 0);
                }
                return baseImage;
            } else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
    }
}
