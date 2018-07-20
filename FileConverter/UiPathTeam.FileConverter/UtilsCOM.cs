using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace UiPathTeam.FileConverter
{
    public static partial class Utils
    {

        //convert existing xls document to xlsx and returns path to file
        public static string ConvertExcel(string FilePathRaw, string NewFileName, string DirectoryToSave,
           string NewFileExtension, Excel.XlFileFormat NewFileFormat)
        {
            

            //calculate file name with exxtension
            if (!NewFileName.EndsWith("." + NewFileExtension)) NewFileName = NewFileName + "." + NewFileExtension;

            //get absolute filePaths for both the new directory and the old file path
            string directoryToSave = String.IsNullOrEmpty(DirectoryToSave)? Directory.GetCurrentDirectory():Path.GetFullPath(DirectoryToSave);
            string xlsFilePath = Path.GetFullPath(FilePathRaw);

            //calculate new File Path
            string newFilePath = Path.Combine(directoryToSave,NewFileName);


            //perform conversion
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Workbook eWorkbook = excelApp.Workbooks.Open(xlsFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            eWorkbook.SaveAs(newFilePath, NewFileFormat, //Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing);



            eWorkbook.Close(false, Type.Missing, Type.Missing);
            excelApp.Quit();
            GC.Collect();
            /* Excel.Workbook openWorkBook = null;

             Excel.Application activeExcel = new Excel.Application();
             openWorkBook = activeExcel.Workbooks.Open(FilePathRaw);
             openWorkBook.SaveAs(newFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);*/


            return newFilePath;
        }


        public static string ConvertWord(string DocFilePathRaw, string NewFileName, string DirectoryToSaveRaw,
           string NewFileExtension, Word.WdSaveFormat NewFileFormat)
        {

            //calculate file name with exxtension
            if (!NewFileName.EndsWith("." + NewFileExtension)) NewFileName = NewFileName + "." + NewFileExtension;

            //get absolute filePaths for both the new directory and the old file path
            string directoryToSave = String.IsNullOrEmpty(DirectoryToSaveRaw) ? Directory.GetCurrentDirectory() : Path.GetFullPath(DirectoryToSaveRaw);
            string docFilePath = Path.GetFullPath(DocFilePathRaw);

            //calculate new File Path
            string newFilePath = Path.Combine(directoryToSave, NewFileName);

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
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            word_doc.SaveAs(newFilePath, NewFileFormat,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            //Close the document and the App
            word_doc.Close(save_changes, Type.Missing, Type.Missing);
            word_app.Quit();
            GC.Collect();

            return newFilePath;
        }

    }
}
