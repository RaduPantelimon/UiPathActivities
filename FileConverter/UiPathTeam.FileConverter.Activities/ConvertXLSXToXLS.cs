using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Activities;
using System.Activities.XamlIntegration;
using System.ComponentModel;


using Excel = Microsoft.Office.Interop.Excel;

namespace UiPathTeam.FileConverter.Activities
{

    [DisplayName("Convert XLSX To XLS")]
    public class ConvertXLSXToXLS : ConversionActivityBaseClass
    {
        public ConvertXLSXToXLS()
        {
            FileExtensionPath = "." + FileTypes.OldExcel;
            OldFileExtension = "Excel Files|*" + FileTypes.NewExcel;
        }

        protected override void Execute(CodeActivityContext context)
        {
            //retrieve the parameters from the Context
            string oldFilePath = OldFilePath.Get(context);
            string newFileName = NewFileName.Get(context);
            string directoryToSave = DirectoryToSave.Get(context);

            //convert and set result
            string resultingFilePath = Utils.ConvertExcel("C:\\UiPath\\ResultXLSX.xlsx", "Blabla", "C:\\UiPath",
                  FileTypes.OldExcel, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);
            ResultingFilePath.Set(context, resultingFilePath);
        }

        
    }
}
