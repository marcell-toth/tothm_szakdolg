using OpenCvSharp;

namespace tothm_szak.ProcessMethods
{
    internal interface IClassification
    {
        /// <summary>
        /// Interface defining the base methods and properties of classification classes
        /// </summary>
        
        // property saving the last loaded image used as base
        Mat? baseImage { get; set; }

        // method taking an OpenCV Mat type as input(base image)
        // and returning an OpenCV Mat type as output(processed image)
        Mat processAndReturnImage(Mat source);
    }
}