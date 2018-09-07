using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPathTeam.WpfFormCreator
{
    public static class Utils
    {

        public static  Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        //if the file is inside our current directory, we remove the absolute part of the path
        public static string TrimFilePath(string initialPath, string absolutePath)
        {
            if(initialPath.StartsWith(absolutePath))
            {
                return initialPath.Remove(0, absolutePath.Length).TrimStart('\\');
            }

            return initialPath;
        }

        public static void ReturnFullPath()
        { }
    }
}
