using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Activities;
using System.Activities.XamlIntegration;
using System.ComponentModel;


namespace UiPathTeam.FileConverter.TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestWorkflow();



            Utils.ConvertToDOCX("C:\\UiPath\\FileConverter\\DSD_CA_MEX_v1.0_.doc","jeg", "C:\\UiPath\\FileConverter");
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
