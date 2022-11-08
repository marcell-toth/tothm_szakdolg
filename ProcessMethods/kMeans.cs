using OpenCvSharp;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class kMeans
    {
        public Mat? baseImage { get; set; }

        /// <summary>
        /// K-Means clustering method
        /// </summary>
        /// <param name="source"></param>
        /// Source image
        /// <param name="k"></param>
        /// Number of clusters,
        /// sets the amount of colors that will be present on the output image
        /// <param name="maxCount"></param>
        /// Termination Criteria, sets max iteration for every element
        /// <param name="epsilon"></param>
        /// Termination Criteria, sets required accuracy for every element
        /// <param name="attempts"></param>
        /// Sets how many times the process is rerun, due to the semi-random nature of the clustering,
        /// running it multiple times and selecting the run with the least total variation for every element might be ideal
        /// Variable returning the overall variation is returned as a double in Cv2.Kmeans
        /// <returns></returns>

        public Mat processAndReturnImage(Mat source, int k = 16, int maxCount = 100, double epsilon = 1.0, int attempts = 1)
        {
            //megadott source kép ellenőrzése
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();

                //kép HSV színtérre váltása
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2HSV);

                //kép formázása egy 3xH*W mátrixba
                //3 színcsatorna és a kép összes pixele egy vonalban
                Mat vectorImg = processedImage.Reshape(3, processedImage.Rows * processedImage.Cols);

                //kép float-ra váltása
                vectorImg.ConvertTo(vectorImg, MatType.CV_32FC3);

                //bemenő kép(színcsatorna x H * W alakban), a kért csomópontok száma, pixel címkézés kimenete
                //leállási kritéria(futási szám(int), pontosság(double)), futások száma,
                //középpontválasztási mód, csomópontok kimenete
                Mat labels = new Mat();
                Mat centers = new Mat();
                Cv2.Kmeans(vectorImg, k, labels, TermCriteria.Both(maxCount, epsilon), attempts, KMeansFlags.PpCenters, centers);

                //kimeneti kép létrehozása az eredeti H x W adatok szerint,
                //8UC3 => unsigned byte(8bit), 3 channel => [0, 0, 0] - [255, 255, 255]
                Mat fullImg = new Mat(processedImage.Rows, processedImage.Cols, MatType.CV_8UC3);

                //eredeti kép minden pixelént átfutás
                for (int i = 0; i < processedImage.Rows; i++)
                {
                    for (int j = 0; j < processedImage.Cols; j++)
                    {
                        //ID => eredeti(H x W) és vektor(H * W) kép pixeleinek megfeleltetése
                        int clusterIndex = labels.At<int>(i * processedImage.Cols + j, 0);

                        //új pixel változó
                        Vec3b ujPixel = new Vec3b
                        {
                            //csomópontok(float) adatainak (byte) typusra váltása az adott pixelértékre
                            Item0 = (byte)centers.At<float>(clusterIndex, 0), // B
                            Item1 = (byte)centers.At<float>(clusterIndex, 1), // G
                            Item2 = (byte)centers.At<float>(clusterIndex, 2), // R
                        };

                        //pixel változó hozzáadása a kimeneti képhez
                        fullImg.Set<Vec3b>(i0: i, i1: j, ujPixel);
                    }
                }

                //kimeneti kép BGR színtérre váltása, mivel már (byte) alapú pixeleket tárol, nem (float)
                Cv2.CvtColor(fullImg, fullImg, ColorConversionCodes.HSV2BGR);

                return fullImg;
            }
            else
            {
                //ha a megadott kép null vagy empty,
                //egy 256x256 fekete kép kerül visszaküldésre
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
    }
}
