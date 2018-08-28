using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Activities;
using System.Activities.XamlIntegration;
using System.ComponentModel;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace UiPathTeam.FileConverter.TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestWorkflow();


            
             Utils.ConvertExcel("C:\\UiPath\\ResultXLSX.xlsx", "Blabla", "C:\\UiPath",
                 "xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);

             Utils.ConvertExcel("C:\\UiPath\\Blabla.xls", "Blabla", "C:\\UiPath",
                "xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            
            
            Utils.ConvertWord("C:\\UiPath\\FileConverter\\jeg.doc", "jeg", "C:\\UiPath\\FileConverter",
                "docx", Word.WdSaveFormat.wdFormatDocumentDefault);
            
            Utils.ConvertWord("C:\\UiPath\\DSD_CA_MEX_v1.0_.docx", "jeg", "C:\\UiPath\\FileConverter",
                            "doc", Word.WdSaveFormat.wdFormatDocument);


            Utils.ConvertWord("C:\\UiPath\\FileConverter\\jeg.doc", "jeg1", "C:\\UiPath\\FileConverter",
               "pdf", Word.WdSaveFormat.wdFormatDocument);

            Utils.ConvertWord("C:\\UiPath\\DSD_CA_MEX_v1.0_.docx", "jeg2", "C:\\UiPath\\FileConverter",
                            "pdf", Word.WdSaveFormat.wdFormatPDF);

            //Utils.ConvertWord("C:\\UiPath\\FileConverter\\DSD_CA_MEX_v1.0_.doc","jeg", "C:\\UiPath\\FileConverter");
        }


        static public void TestWorkflow()
        {
            ActivityXamlServicesSettings settings = new ActivityXamlServicesSettings
            {
                CompileExpressions = true
            };

            Activity workflow = ActivityXamlServices.Load("FileConverterTest.xaml", settings);
            WorkflowInvoker.Invoke(workflow);

            Console.WriteLine("Press <enter> to exit");
            Console.ReadLine();
        }
    }
}
