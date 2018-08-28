using System;
using System.IO;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace UiPathTeam.FileConverter
{
    public static partial class Utils
    {
        /// <summary>  
        ///  Convert existing Excel document to a specified extension and returns path to file
        /// </summary>  
        public static string ConvertExcel(string FilePathRaw, string NewFileName, string DirectoryToSave,
           string NewFileExtension, Excel.XlFileFormat NewFileFormat)
        {
            //calculate file name with exxtension
            if (!NewFileName.EndsWith("." + NewFileExtension)) NewFileName = NewFileName + "." + NewFileExtension;

            //get absolute filePaths for both the new directory and the old file path
            string directoryToSave = String.IsNullOrEmpty(DirectoryToSave) ? Directory.GetCurrentDirectory() : Path.GetFullPath(DirectoryToSave);
            string xlsFilePath = Path.GetFullPath(FilePathRaw);

            //calculate new File Path
            string newFilePath = Path.Combine(directoryToSave, NewFileName);

           
                //perform conversion
                Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;

            //performing conversion operation
            try
            {
                Excel.Workbook eWorkbook = excelApp.Workbooks.Open(xlsFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                eWorkbook.SaveAs(newFilePath, NewFileFormat,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

                eWorkbook.Close(false, Type.Missing, Type.Missing);
            }
            catch
            {
                throw;
            }
            finally
            {
                //Close the App
                excelApp.Quit();
                GC.Collect();
            }

            return newFilePath;
        }

        /// <summary>
        ///  Convert existing Word document to a specified extension and returns path to file
        /// </summary> 
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

           

            // Open and init the Word Application.
            Word._Application word_app = new Word.Application();
            word_app.Visible = false;
            word_app.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;
            bool save_changes = false;


            //performing conversion operation
            try
            {
                //initialize settings
                bool confirm_conversions = false;
                bool read_only = true;
                bool add_to_recent_files = false;
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

                //Close the document
                word_doc.Close(save_changes, Type.Missing, Type.Missing);

            }
            catch
            {
                throw;
            }
            finally
            {
                //Close the App
                word_app.Quit();
                GC.Collect();
            }
            return newFilePath;
        }

    }
}
