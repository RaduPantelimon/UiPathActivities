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
    public abstract class ConversionActivityBaseClass:CodeActivity
    {
        [Category("Input")]
        [DisplayName("File Path")]
        [Description("File Path of the Document to be converted")]
        [RequiredArgument]
        public InArgument<string> OldFilePath { get; set; }

        [Category("Input")]
        [DisplayName("New File Name")]
        [Description("Name of the New File. Without the extension.")]
        [RequiredArgument]
        public InArgument<string> NewFileName { get; set; }

        [Category("Input")]
        [DisplayName("Directory To Save In")]
        [Description("Name of the New File. Without the extension.")]
        public InArgument<string> DirectoryToSave { get; set; }

        [Category("Output")]
        [DisplayName("Resulting FilePath")]
        [Description("Complete Path of the resulting File. Calculated from Directory to Save In and New File Name")]
        public OutArgument<string> ResultingFilePath { get; set; }

        public string FileExtensionPath { get; set; }
        public string OldFileExtension { get; set; }

        //public static string WildCard = "*";

        protected override void Execute(CodeActivityContext context)
        {
            throw new NotImplementedException();
        }
    }
}
