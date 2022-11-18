using OpenCvSharp;

namespace tothm_szak.ProcessMethods
{
    internal class cannyEdgeDetection : IClassification
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            // megadott source kép ellenőrzése
            if (source != null && !source.Empty())
            {
                baseImage = source;
                return applyEdgeDetection(source);
            } else
            {
                //ha a megadott kép null vagy empty,
                //egy 256x256 fekete kép kerül visszaküldésre
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }

        private Mat applyEdgeDetection(Mat source, int t1 = 70, int t2 = 95, int aptSize = 3)
        {
            Mat processedImage = new Mat();

            // kép fekete/fehérre váltása
            Cv2.CvtColor(source, processedImage, ColorConversionCodes.BGR2GRAY);

            // kimenet javításához elmosás
            Cv2.GaussianBlur(processedImage, processedImage, new Size(3, 3), 0, 0, BorderTypes.Default);

            // éldetektálás alkalmazása
            Cv2.Canny(processedImage, processedImage, t1, t2, aptSize, true);
            return processedImage;
        }
    }
}
