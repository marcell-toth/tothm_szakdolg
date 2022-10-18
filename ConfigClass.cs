using System;
using System.Collections.Generic;
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
        
        
    }
}
