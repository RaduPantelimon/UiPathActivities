using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.XLExcel
{
    public static partial class Utils
    {

        //convert existing xls document to xlsx and returns path to file
        public static string ConvertToXLSX(string xlsFilePath, string newFileName, string directoryToSave)
        {
            string newFilePath = "";

            //calculate file name with exxtension
            if (!newFileName.EndsWith(".xlsx")) newFileName = newFileName + ".xlsx";


            //calculate new File Path
            newFilePath = Path.Combine(directoryToSave,newFileName);


            //perform conversion
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Workbook eWorkbook = excelApp.Workbooks.Open(xlsFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            eWorkbook.SaveAs(newFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing);
            eWorkbook.Close(false, Type.Missing, Type.Missing);


            return newFilePath;
        }
    }
}
