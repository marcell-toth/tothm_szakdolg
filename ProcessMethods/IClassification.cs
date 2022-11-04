using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using OpenCvSharp;

namespace tothm_szak.ProcessMethods
{
    internal interface IClassification
    {
        Mat? baseImage { get; set; }
        Mat processAndReturnImage(Mat source);
    }
}