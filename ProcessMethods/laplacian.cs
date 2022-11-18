using OpenCvSharp;

namespace tothm_szak.ProcessMethods
{
    internal class laplacian : IClassification
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            //megadott source kép ellenőrzése
            if (source != null && !source.Empty())
            {
                baseImage = source;

                return applyLaplacian(source);
            } else
            {   
                //ha a megadott kép null vagy empty,
                //egy 256x256 fekete kép kerül visszaküldésre
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }

        private Mat applyLaplacian(Mat source)
        {
            Mat processedImage = new Mat();

            // kép átváltása szürkére
            Cv2.CvtColor(source, processedImage, ColorConversionCodes.BGR2GRAY);

            // Laplacian éldetektálás alkalmazása
            Cv2.Laplacian(processedImage, processedImage, MatType.CV_8UC1, 3, 1, 0, BorderTypes.Default);
            return processedImage;
        }
    }
}
