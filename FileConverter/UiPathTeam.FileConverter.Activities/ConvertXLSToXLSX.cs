using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using System.Data;


namespace UiPathTeam.FileConverter.Activities
{
    public class ConvertXLSToXLSX:CodeActivity
    {

        [Category("Input")]
        [DisplayName("XLS FilePath")]
        [RequiredArgument]
        public InArgument<string> OldFilePath { get; set; }

        [Category("Input")]
        [DisplayName("New File Name")]
        public InArgument<string> NewFileName { get; set; }

        [Category("Input")]
        [DisplayName("Directory To Save In")]
        public InArgument<string> DirectoryToSave { get; set; }

        [Category("Output")]
        [DisplayName("Resulting FilePath")]
        public OutArgument<string> ResultingFilePath { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            //retrieve the parameters from the Context
            string oldFilePath = OldFilePath.Get(context);
            string newFileName = NewFileName.Get(context);
            string directoryToSave = DirectoryToSave.Get(context);


            //convert and set result
            string resultingFilePath = Utils.ConvertToXLSX(oldFilePath, newFileName, directoryToSave);
            ResultingFilePath.Set(context, resultingFilePath);
        }
    }
}
