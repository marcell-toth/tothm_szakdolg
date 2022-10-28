using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tothm_szak
{
    public static class ConfigClass
    {
        //technically global variables violate OOP principles,
        //this is a temporary solution
        //try to replace later with OOP

        /// <summary>
        /// Static class for storing global variables used through the application
        /// Contains the path to the folder containing the sampled images,
        /// a string list, the selected process mode, possible process modes
        /// and the allowed file extensions
        /// </summary>

        public static string folderPath = "";

        public static IEnumerable<string> ImgPath = Enumerable.Empty<string>();

        public static processMode activeProcessMode = processMode.None;
        public enum processMode
        {
            None,
            Grayscale,
            SimpleThreshold,
            AdaptiveThreshold,
            OtsuThreshold,
            BradleyThreshold,
            Laplace,
            Canny,
            Contour,
            SelectiveSearch
        }

        //allowed file extensions
        public static bool isAllowedPng = true;
        public static bool isAllowedJpeg = true;
        public static bool isAllowedJpg = true;
        
    }
}
