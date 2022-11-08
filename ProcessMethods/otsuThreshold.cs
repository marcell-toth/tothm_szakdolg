using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class otsuThreshold : IClassification
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
            Mat processedImage = new Mat();

            // input kép szürkére alakítása
            Cv2.CvtColor(source, processedImage, ColorConversionCodes.BGR2GRAY);

            // elmosás a kimenet javításához
            Cv2.GaussianBlur(processedImage, processedImage, new Size(3, 3), 0, 0, BorderTypes.Default);

            // küszöb alkalmazása
            Cv2.Threshold(processedImage, processedImage, thresh, 255, ThresholdTypes.Otsu);

            return processedImage;
        }
    }
}
