
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class findContours : IClassification
    {
        public Mat? baseImage { get; set; }
        public Mat processAndReturnImage(Mat source)
        {
            if (source != null && !source.Empty())
            {
                //adaptiveThreshold aT = new();
                baseImage = source;
                Mat processedImage = new Mat();

                Cv2.CvtColor(baseImage, processedImage, ColorConversionCodes.BGR2GRAY);
                Cv2.GaussianBlur(processedImage, processedImage, new Size(3, 3), 0, 0, BorderTypes.Default);
                Cv2.Threshold(processedImage, processedImage, 150, 255, ThresholdTypes.Otsu);
                //Cv2.MedianBlur(src, src, 3);
                //src = aT.adaptiveThresholdImg(src);
                OpenCvSharp.Point[][] pl;
                HierarchyIndex[] hi;


                Cv2.FindContours(processedImage, out pl, out hi, RetrievalModes.List, ContourApproximationModes.ApproxNone, null);
                Array.Sort(pl, (x, y) => Cv2.ContourArea(x).CompareTo(Cv2.ContourArea(y)));

                processedImage = baseImage;
                Cv2.DrawContours(processedImage, pl.Where(x => Cv2.ContourArea(x) > 5), -1, new Scalar(0, 0, 255), 1, LineTypes.Link8);

                return processedImage;
            } else
            {
                return new Mat(256, 256, MatType.CV_8UC1, 0);
            }
        }
         
        private bool ContourSizeCheck(OpenCvSharp.Point[] x)
        {
            if (Cv2.ContourArea(x) > 5)
            {
                return true;
            } else
            {
                return false;
            }
        }
        private OpenCvSharp.Point[][] sortConts(OpenCvSharp.Point[][] contours)
        {
            List<OpenCvSharp.Point[]> contourList = contours.ToList();
            contourList.Sort((OpenCvSharp.Point[] x, OpenCvSharp.Point[] y) => CompareBySize(x, y));
            for (int i=0; i < contours.GetLength(0); i++)
            {
                for (int j = 0; j < contours.GetLength(0); j++)
                {
                    
                }
                
            }
            return contours;
        }

        //this is awful but it somehow works
        public static int CompareBySize(OpenCvSharp.Point[] x, OpenCvSharp.Point[] y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                } else
                {
                    return -1;
                }
            } else
            {
                if (y == null)
                {
                    return 1;
                }    else
                {
                    if (Cv2.ContourArea(x) > Cv2.ContourArea(y))
                    {
                        return 1;
                    } else
                    {
                        return -1;
                    }
                }
            }
        }
        

    }
}
