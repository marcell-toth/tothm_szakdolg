using OpenCvSharp;

namespace tothm_szak.ProcessMethods
{
    internal class simpleThreshold : IClassification
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            //megadott source kép ellenőrzése
            if (source != null && !source.Empty())
            {
                baseImage = source;

                return applyThreshold(source);
            } else
            {
                //ha a megadott kép null vagy empty,
                //egy 256x256 fekete kép kerül visszaküldésre
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }

        // küszöbértékelés alkalmazása
        private Mat applyThreshold(Mat source, int thresh = 150)
        {
            Mat processedImage = new Mat(source.Rows, source.Cols, MatType.CV_8UC1);

            // input kép szürkére alakítása
            Cv2.CvtColor(source, processedImage, ColorConversionCodes.BGR2GRAY);

            for (int i = 0; i < source.Rows; i++)
            {
                for (int j = 0; j < source.Cols; j++)
                {
                    if (source.At<byte>(i, j) > thresh)
                    {
                        processedImage.At<byte>(i, j) = byte.MaxValue;
                    } else
                    {
                        processedImage.At<byte>(i, j) = 0;
                    }
                }
            }

            return processedImage;
        }

        // küszöbértékelás alkalmazása meghívással
        private Mat applyThresholdCall(Mat source, int thresh = 150)
        {
            Mat processedImage = new Mat();

            // input kép szürkére alakítása
            Cv2.CvtColor(source, processedImage, ColorConversionCodes.BGR2GRAY);

            // küszöb alkalmazása
            Cv2.Threshold(processedImage, processedImage, thresh, 255, ThresholdTypes.Binary);
            return processedImage;
        }
    }
}
