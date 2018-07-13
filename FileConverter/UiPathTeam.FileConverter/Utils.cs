using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPathTeam.FileConverter
{
    public static partial class Utils
    {

        //if the file is inside our current directory, we remove the 
        public static string TrimFilePath(string initialPath, string absolutePath)
        {
            if (initialPath.StartsWith(absolutePath))
            {
                return initialPath.Remove(0, absolutePath.Length).TrimStart('\\');
            }

            return initialPath;
        }


      
    }
}
