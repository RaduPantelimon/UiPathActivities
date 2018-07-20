using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using System.Data;

using Word = Microsoft.Office.Interop.Word;

namespace UiPathTeam.FileConverter.Activities
{

    [DisplayName("Convert Word To PDF")]
    public class ConvertToPDF : ConversionActivityBaseClass
    {
        public ConvertToPDF()
        {
            FileExtensionPath = "." + FileTypes.PDFExtension;
            OldFileExtension = "Word Files|*" + FileTypes.NewWord +
                ";*" + FileTypes.OldWord;
        }

        protected override void Execute(CodeActivityContext context)
        {
            //retrieve the parameters from the Context
            string oldFilePath = OldFilePath.Get(context);
            string newFileName = NewFileName.Get(context);
            string directoryToSave = DirectoryToSave.Get(context);

            //convert and set result
            string resultingFilePath = Utils.ConvertWord(oldFilePath, newFileName, directoryToSave, FileTypes.PDFExtension,
                 Word.WdSaveFormat.wdFormatPDF);
            ResultingFilePath.Set(context, resultingFilePath);
        }
    }
}
