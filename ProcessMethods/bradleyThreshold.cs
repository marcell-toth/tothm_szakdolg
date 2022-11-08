using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;


namespace tothm_szak.ProcessMethods
{
    internal class bradleyThreshold
    {
        /// <summary>
        /// Implementation of the Bradley Roth method of image segmentation
        ///  Method utilizes the integral image of the input to reduce 
        ///  the complexity of the reguired conputations.
        ///  Integral of the image first needs to be calculated,
        ///  meaning the method cycles through the given input multiple times.
        /// </summary>
        public Mat? baseImage { get; set; }


        public Mat processAndReturnImage(Mat source)
        {
            // megadott source kép ellenőrzése
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();

                //input kép fekete/fehérré alakítása (C3 => C1)
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);

                processedImage = bradleyThresholdProc(baseImage);
                return processedImage;
            } else
            {
                //ha a megadott kép null vagy empty,
                //egy 256x256 fekete kép kerül visszaküldésre
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }

        /// <summary>
        /// Bradley Roth Thresholding alkalmazása
        /// </summary>
        /// <param name="source"></param>
        /// Bemeneti kép
        /// <param name="s"></param>
        /// Vizsgált ablak mérete default(61x61)
        /// <param name="t"></param>
        /// Az ablakban mért átlag alatti legkissebb megengedett érték (%)
        /// Ha az s ablakban mért átlag alatt van a vizsgált érték legalább t %-al, akkor fekete
        /// <returns></returns>
        private Mat bradleyThresholdProc(Mat source, int s = 61, int t = 15)
        {
            // kép integrál számítása
            Mat intImg = integralImage(source);

            // bemenettel megegyező méretű, fekete/fehér(C1) kép létrehozása
            Mat outImg = new Mat(source.Rows, source.Cols, MatType.CV_8UC1);

            for (int i = 0; i < source.Rows; i++)
            {
                for (int j = 0; j < source.Cols; j++)
                {
                    // pixel körüli ablak sarkai, s/2 távolságra
                    int x1 = i - s / 2;
                    int x2 = i + s / 2;
                    int y1 = j - s / 2;
                    int y2 = j + s / 2;

                    //kép szélén túlnyúló ablak sarkok lekezelése
                    if (x1 < 0) { x1 = 0; }
                    if (x2 > source.Rows - 1) { x2 = source.Rows - 1; }

                    if (y1 < 0) { y1 = 0; }
                    if (y2 > source.Cols - 1) { y2 = source.Cols - 1; }

                    //pixel körüli s méretű ablakban található képek száma
                    //mivel a kép szélén változhat az ablak mérete az alap s*s-től, így ezt az eltérést figyelni kell
                    int count = (x2 - x1 + 1) * (y2 - y1 + 1);

                    // pixel körüli ablakban lévő értékek összege
                    // a kép integráljának használatával ez 4 elem(sarkok) összeadás/kivonásával megkapható
                    int sum = intImg.At<int>(x2, y2) - intImg.At<int>(x2, y1) - intImg.At<int>(x1, y2) + intImg.At<int>(x1, y1);

                    //pixel értékének a körülötte lévő ablak értékeinek átlagához viszonyítása
                    // ((100 - t) / 100) megadja a megengedett százalékban vett eltérést
                    if ((source.At<byte>(i, j) * count) < (sum * (100 - t) / 100))
                    {
                        outImg.At<byte>(i, j) = 0;
                    }
                    else
                    {
                        outImg.At<byte>(i, j) = 255;
                    }
                }
            }
            return outImg;
        }

        // input kép integráljáak számítása
        // a metódus összeadja(integrálja) a kép összes elemének az értékét
        // balról jobbra, fentről lefele haladva, így minden elemnek,
        // a tőle balra vagy felette lévők csak nála kissebbek lehetnek
        //lehetővé téve akármilyen ablak elemeinek összeadását csak az ablak 4 sarkát felhasználva
        private Mat integralImage(Mat source)
        {
            // képpel megegyező méretű kimenet létrehozása
            Mat intImg = new Mat(source.Rows, source.Cols, MatType.CV_32SC1);

            // pixelenkénti bejárás
            for (int i = 0; i < source.Rows; i++)
            {
                int sum = 0;
                for (int j = 0; j < source.Cols; j++)
                {
                    sum = sum + source.At<byte>(i, j);
                    if (i == 0)
                    {
                        intImg.At<int>(i, j) = sum;
                    }
                    else
                    {
                        intImg.At<int>(i, j) = intImg.At<int>(i - 1, j) + sum;
                    }
                }
            }
            return intImg;
        }
    }
}
