using OpenCvSharp.Aruco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tothm_szak.Pages;

namespace tothm_szak
{
    public static class ConfigClass
    {
        // technically global variables violate OOP principles,
        // this is a temporary solution
        // try to replace later with OOP

        // after 2 weeks, this is a forever temporary solution
        // :C

        /// <summary>
        /// Static class for storing global variables used through the application
        /// Contains the path to the folder containing the sampled images,
        /// a string list, the selected process mode, possible process modes
        /// and the allowed file extensions
        /// </summary>

        // selected folder's path
        public static string folderPath = "";

        // string enumerable stores the path to
        // each of images with the correct file type
        public static IEnumerable<string> ImgPath = Enumerable.Empty<string>();

        // variable storing the currently selected processing method
        public static processMode activeProcessMode = processMode.None;

        // enum containing all the selectable processing methods
        public enum processMode
        {
            [Description ("Semmi")]
            None,
            [Description("Szürkekép")]
            Grayscale,
            [Description("Egyszerű küszöb")]
            SimpleThreshold,
            [Description("Adaptív küszöb")]
            AdaptiveThreshold,
            [Description("Otsu küszöb")]
            OtsuThreshold,
            [Description("Bradley Roth küszöb")]
            BradleyThreshold,
            [Description("Laplace él")]
            Laplace,
            [Description("Canny él")]
            Canny,
            [Description("Kontúr detektálás")]
            Contour,
            [Description("Selective Search")]
            SelectiveSearch,
            [Description("K-Means klaszter")]
            KMeans
        }

        // enum containing all the test options
        public enum testType
        {
            singleTest, 
            folderTest
        }

        // storing the test options and their on/off state
        public static Dictionary<testType, bool> testModes = new Dictionary<testType, bool>
        {
            // setting default values matching the checkboxes
            {testType.singleTest, true },
            {testType.folderTest, true }
        };

        // storing the parameters for the test going through the whole folder
        // cycleNum is the number of times we go through the folder, minimum 1
        // waitNum is the wait between images while iterating,
        // wait does not get counted into the final time
        public static int cycleNum = 1;
        public static int waitNum = 1;

            

        // allowed file extensions
        public static bool isAllowedPng = true;
        public static bool isAllowedJpeg = true;
        public static bool isAllowedJpg = true;
        
    }
}
