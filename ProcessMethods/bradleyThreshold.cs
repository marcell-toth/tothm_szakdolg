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
        public Mat bradleyThresholdImg(Mat src)
        {
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
        }
    }
}
