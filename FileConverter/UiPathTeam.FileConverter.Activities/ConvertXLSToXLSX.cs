using System.Activities;
using System.ComponentModel;


namespace UiPathTeam.FileConverter.Activities
{
    [DisplayName("Convert XLS To XLSX")]
    public class ConvertXLSToXLSX : ConversionActivityBaseClass
    {
        public ConvertXLSToXLSX()
        {
            FileExtensionPath = "." + FileTypes.NewExcel;
            OldFileExtension = "Excel Files|*" + FileTypes.OldExcel;
        }

        protected override void Execute(CodeActivityContext context)
        {
            //retrieve the parameters from the Context
            string oldFilePath = OldFilePath.Get(context);
            string newFileName = NewFileName.Get(context);
            string directoryToSave = DirectoryToSave.Get(context);

            //convert and set result
            string resultingFilePath = Utils.ConvertExcel(oldFilePath, newFileName, directoryToSave, FileTypes.NewExcel, 
                Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            ResultingFilePath.Set(context, resultingFilePath);
        }
    }
}
