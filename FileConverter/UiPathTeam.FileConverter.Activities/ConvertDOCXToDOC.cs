﻿using System.Activities;
using System.ComponentModel;


using Word = Microsoft.Office.Interop.Word;

namespace UiPathTeam.FileConverter.Activities
{

    [DisplayName("Convert DOCX To DOC")]
    public class ConvertDOCXToDOC : ConversionActivityBaseClass
    {

        public ConvertDOCXToDOC()
        {
            FileExtensionPath = "." + FileTypes.OldWord;
            OldFileExtension = "Word Files|*" + FileTypes.NewWord;
        }

        protected override void Execute(CodeActivityContext context)
        {
            //retrieve the parameters from the Context
            string oldFilePath = OldFilePath.Get(context);
            string newFileName = NewFileName.Get(context);
            string directoryToSave = DirectoryToSave.Get(context);

            //convert and set result
            string resultingFilePath = Utils.ConvertWord(oldFilePath, newFileName, directoryToSave, FileTypes.OldWord,
                Word.WdSaveFormat.wdFormatDocument);
            ResultingFilePath.Set(context, resultingFilePath);
        }
    }
}
