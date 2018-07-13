using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Word = Microsoft.Office.Interop.Word;

namespace UiPathTeam.FileConverter
{
    public static partial class Utils
    {

        //convert existing xls document to xlsx and returns path to file
        public static string ConvertToXLSX(string xlsFilePathRaw, string newFileName, string directoryToSaveRaw)
        {
            

            //calculate file name with exxtension
            if (!newFileName.EndsWith(".xlsx")) newFileName = newFileName + ".xlsx";

            //get absolute filePaths for both the new directory and the old file path
            string directoryToSave = String.IsNullOrEmpty(directoryToSaveRaw)? Directory.GetCurrentDirectory():Path.GetFullPath(directoryToSaveRaw);
            string xlsFilePath = Path.GetFullPath(xlsFilePathRaw);

            //calculate new File Path
            string newFilePath = Path.Combine(directoryToSave,newFileName);


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


        public static string ConvertToDOCX(string docFilePathRaw, string newFileName, string directoryToSaveRaw)
        {

            //calculate file name with exxtension
            if (!newFileName.EndsWith(".docx")) newFileName = newFileName + ".docx";

            //get absolute filePaths for both the new directory and the old file path
            string directoryToSave = String.IsNullOrEmpty(directoryToSaveRaw) ? Directory.GetCurrentDirectory() : Path.GetFullPath(directoryToSaveRaw);
            string docFilePath = Path.GetFullPath(docFilePathRaw);

            //calculate new File Path
            string newFilePath = Path.Combine(directoryToSave, newFileName);

            // Open the Word Application.
            Word._Application word_app = new Word.Application();
            word_app.Visible = false;
            word_app.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;

            //initialize settings
            bool confirm_conversions = false;
            bool read_only = true;
            bool add_to_recent_files = false;
            bool save_changes = false;
            object format = 0;

            //open the document
            Word._Document word_doc = word_app.Documents.Open(docFilePath,confirm_conversions,  read_only,add_to_recent_files, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Word.WdSaveFormat.wdFormatDocument, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            word_doc.SaveAs(newFilePath, Word.WdSaveFormat.wdFormatDocumentDefault,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            // Close the document without prompting.
            word_doc.Close(save_changes, Type.Missing, Type.Missing);

            return newFilePath;
        }

    }
}
