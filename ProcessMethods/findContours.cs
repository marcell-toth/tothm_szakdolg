
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak.ProcessMethods
{
    internal class findContours
    {
        public Mat findContImg(Mat src)
        {
            adaptiveThreshold aT = new();
            Mat dst = src.Clone();
            Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
            src = aT.adaptiveThresholdImg(src);
            //src = simpleThreshold(src);
            OpenCvSharp.Point[][] pl;
            HierarchyIndex[] hi;
            Scalar sc = new Scalar(0, 0, 255);

            Cv2.FindContours(src, out pl, out hi, RetrievalModes.Tree, ContourApproximationModes.ApproxNone, null);
            //pl.Sort((OpenCvSharp.Point[] x, OpenCvSharp.Point[] y) => IComparer.Compare(Cv2.ContourArea(pl[x]) > Cv2.ContourArea(pl[x])));
            //sortConts(pl);
            //pl = (OpenCvSharp.Point[][])pl.Where(x => Cv2.ContourArea(x) > 5);        wtf
            //pl = (OpenCvSharp.Point[][])pl.Where(x => x.GetType() == OpenCvSharp.Point[]);

            Cv2.DrawContours(dst, pl, -1, sc, 1, LineTypes.Link8);

            return dst;
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
