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
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                baseImage = source;
                Mat processedImage = new Mat();
                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);

                processedImage = bradleyThresholdProc(baseImage);
                return processedImage;
            } else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
            /*
            // flips img in grayscale
            // mainly a test for pixel get/set manipulation
            int i = 0;
            while (i < src.Cols / 2)
            {
                for (int j = 0; j < src.Rows; j++)
                {
                    byte hp = src.At<byte>(j, src.Cols - i);
                    src.At<byte>(j, src.Cols - i) = src.At<byte>(j, i);
                    src.At<byte>(j, i) = hp;
                }
                i++;
            }
            return src;
            */
        }

        private Mat bradleyThresholdProc(Mat source, int s = 61, int t = 15)
        {
            Mat intImg = integralImage(source);
            Mat outImg = new Mat(source.Rows, source.Cols, MatType.CV_8UC1);

            for (int i = 0; i < source.Rows; i++)
            {
                for (int j = 0; j < source.Cols; j++)
                {
                    int x1 = i - s / 2;
                    int x2 = i + s / 2;
                    int y1 = j - s / 2;
                    int y2 = j + s / 2;

                    if (x1 < 0) { x1 = 0; }
                    if (x2 > source.Rows - 1) { x2 = source.Rows - 1; }

                    if (y1 < 0) { y1 = 0; }
                    if (y2 > source.Cols - 1) { y2 = source.Cols - 1; }

                    int count = (x2 - x1 + 1) * (y2 - y1 + 1);
                    int sum = intImg.At<int>(x2, y2) - intImg.At<int>(x2, y1) - intImg.At<int>(x1, y2) + intImg.At<int>(x1, y1);

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
        private Mat integralImage(Mat source)
        {
            Mat intImg = new Mat(source.Rows, source.Cols, MatType.CV_32SC1);

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
