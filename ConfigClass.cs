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

        //after 2 weeks, this is a forever temporary solution
        // :C

        /// <summary>
        /// Static class for storing global variables used through the application
        /// Contains the path to the folder containing the sampled images,
        /// a string list, the selected process mode, possible process modes
        /// and the allowed file extensions
        /// </summary>

        //tallózott mappa elérési újta
        public static string folderPath = "";

        //string enumerable tároló a kiválasztott képek elérési útjáról
        public static IEnumerable<string> ImgPath = Enumerable.Empty<string>();

        //aktív feldolgozási módot jelölő változó
        public static processMode activeProcessMode = processMode.None;

        //enum változó ami az összes lehetséges feldolgozási módot tartalmazza
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
            SelectiveSearch,
            KMeans
        }

        //allowed file extensions
        public static bool isAllowedPng = true;
        public static bool isAllowedJpeg = true;
        public static bool isAllowedJpg = true;
        
    }
}
