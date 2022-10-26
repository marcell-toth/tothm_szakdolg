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
            Laplace,
            Contour,
            SelectiveSearch
        }

        //allowed file extensions
        public static bool isAllowedPng = true;
        public static bool isAllowedJpeg = true;
        public static bool isAllowedJpg = true;
        
    }
}
