using OpenCvSharp;
using System.Diagnostics;

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
        private Mat applyThreshold(Mat source, int blocksize = -1, int weight = 15)
        {
            baseImage = source;
            Mat processedImage = new Mat();

            int scale = 2;
            if (blocksize == -1) { 
                blocksize = (source.Rows + source.Cols) / scale * 2;
            } 
            if (blocksize % 2 == 0) { blocksize++; }
            // input kép színterének átváltása fekete/fehérre
            Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);

            // elmosás a kimenet javításához
            //processedImage = processedImage.MedianBlur(3);

            // adaptív küszöbértékelés végrehajtása
            // AdaptiveThresholdTypes: Gaussian/Mean
            // blocksize: futó ablak mérete(blocksize x blocksize)
            // weight: ablakban kapott érték súlyozása(+, -)
            Cv2.AdaptiveThreshold(processedImage, processedImage, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, blocksize, weight);
            return processedImage;
        }

        /// <summary>
        /// Unused, for comparison only
        /// </summary>
        /// <param name="source"></param>
        /// <param name="blocksize"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        private Mat applyThresholdImp(Mat source, int blocksize = 15, int weight = 15)
        {
            // bemenettel megegyező méretű, fekete/fehér(C1) kép létrehozása
            Mat outImg = new Mat(source.Rows, source.Cols, MatType.CV_8UC1);
            //s = (source.Rows + source.Cols) / 16;
            for (int i = 0; i < source.Rows; i++)
            {
                for (int j = 0; j < source.Cols; j++)
                {
                    int sum = 0;
                    int checkedNum = 0;

                    int x1 = i - blocksize / 2;
                    int x2 = i + blocksize / 2;
                    int y1 = j - blocksize / 2;
                    int y2 = j + blocksize / 2;

                    //kép szélén túlnyúló ablak sarkok lekezelése
                    if (x1 < 0) { x1 = 0; }
                    if (x2 > source.Rows - 1) { x2 = source.Rows - 1; }

                    if (y1 < 0) { y1 = 0; }
                    if (y2 > source.Cols - 1) { y2 = source.Cols - 1; }

                    for (int winRow = x1; winRow < x2; winRow++)
                    {
                        for (int winCol = y1; winCol < y2; winCol++)
                        {
                            sum = sum + source.At<byte>(winRow, winCol);
                            checkedNum++;
                        }
                    }
                    if (source.At<byte>(i, j) > (sum / checkedNum) + weight)
                    {
                        outImg.At<byte>(i, j) = 255;
                    }
                    else
                    {
                        outImg.At<byte>(i, j) = 0;
                    }
                }
            }
            return outImg;
        }
    }
}
