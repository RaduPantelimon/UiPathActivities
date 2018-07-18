﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using System.Data;


namespace UiPathTeam.FileConverter.Activities
{
    public class ConvertXLSToXLSX:ConversionActivityBaseClass
    {
        public ConvertXLSToXLSX()
        {
            FileExtensionPath = FileTypes.NewExcel;
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
