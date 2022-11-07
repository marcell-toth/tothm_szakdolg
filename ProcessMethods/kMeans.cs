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
    internal class kMeans : IClassification
    {
        public Mat? baseImage { get; set; }

        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2HSV);
                processedImage.ConvertTo(processedImage, MatType.CV_32FC3);

                Mat vectorImg = processedImage.Reshape(3, processedImage.Rows * processedImage.Cols);


                Mat labels = new Mat();
                Mat centers = new Mat();
                Cv2.Kmeans(vectorImg, 8, labels, TermCriteria.Both(10, 5.0), 3, KMeansFlags.PpCenters, centers);

                Mat fullImg = new Mat(processedImage.Rows, processedImage.Cols, MatType.CV_8UC3);
                for (int i = 0; i < processedImage.Rows; i++)
                {
                    for (int j = 0; j < processedImage.Cols; j++) {
                        int clusterIndex = labels.At<int>(0, i * processedImage.Rows + j);

                        Vec3b newPixel = new Vec3b
                        {
                            Item0 = (byte)centers.At<float>(clusterIndex, 0), // B
                            Item1 = (byte)centers.At<float>(clusterIndex, 1), // G
                            Item2 = (byte)centers.At<float>(clusterIndex, 2) // R
                        };

                        fullImg.At<Vec3b>(i, j) = newPixel;

                    }
                }


                processedImage.ConvertTo(processedImage, MatType.CV_8UC3);
                Cv2.CvtColor(processedImage, processedImage, ColorConversionCodes.HSV2BGR);

                return fullImg;
            }
            else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
    }
}
