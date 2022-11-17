using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class adaptiveThreshold : IClassification
    {
        /// <summary>
        /// Adaptív küszöbértékelés
        /// </summary>
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            // megadott source kép ellenőrzése
            if (source != null && !source.Empty())
            {
                // küszöbértékelés alkalmazása
                return applyThreshold(source);
            } else
            {
                //ha a megadott kép null vagy empty,
                //egy 256x256 fekete kép kerül visszaküldésre
                return (new Mat(256, 256, MatType.CV_8UC1, 0));
            }
        }

        //küszöbértékelés alkalmazása
        private Mat applyThreshold(Mat source, int blocksize = 7, int weight = 15)
        {
            baseImage = source;
            Mat processedImage = new Mat();

            // input kép színterének átváltása fekete/fehérre
            Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);

            // elmosás a kimenet javításához
            processedImage = processedImage.MedianBlur(3);

            // adaptív küszöbértékelés végrehajtása
            // AdaptiveThresholdTypes: Gaussian/Mean
            // blocksize: futó ablak mérete(7x7)
            // weight: ablakban kapott érték súlyozása(+, -)
            Cv2.AdaptiveThreshold(processedImage, processedImage, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blocksize, weight);
            return processedImage;
        }
    }
}
