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

    [DisplayName("Convert DOC To DOCX")]
    public class ConvertDOCToDOCX : ConversionActivityBaseClass
    {
        public ConvertDOCToDOCX()
        {
            FileExtensionPath = "." + FileTypes.NewWord;
            OldFileExtension = "Word Files|*" + FileTypes.OldWord;
        }

        protected override void Execute(CodeActivityContext context)
        {
            //retrieve the parameters from the Context
            string oldFilePath = OldFilePath.Get(context);
            string newFileName = NewFileName.Get(context);
            string directoryToSave = DirectoryToSave.Get(context);

            //convert and set result
            string resultingFilePath = Utils.ConvertWord(oldFilePath, newFileName, directoryToSave, FileTypes.NewWord,
                Word.WdSaveFormat.wdFormatDocumentDefault);
            ResultingFilePath.Set(context, resultingFilePath);
        }
    }
}
