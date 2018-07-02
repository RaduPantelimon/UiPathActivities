using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.XLExcel
{
    public class XLExcelContextInfo
    {
        public string path;
        public string sheetName;

        public static string XLExcelContextInfoTag { get { return "XLExcelContextInfoInfoTag"; } }

        public void WriteProperties()
        {
            Console.WriteLine("path: " + path);
            Console.WriteLine("sheetName: " + sheetName);
        }


    }

}
