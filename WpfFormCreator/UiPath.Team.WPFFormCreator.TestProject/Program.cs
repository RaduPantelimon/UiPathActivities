using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Controls;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;

using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml;


using UiPathTeam.WpfFormCreator;
using System.Activities.XamlIntegration;
using System.Activities;
using System.Windows.Media.Imaging;

namespace UiPath.Team.WPFFormCreator.TestProject
{

    //method for validation - add empty method for now
    class Program
    {
        static private Form mainForm;
        [STAThread]
        static void Main()
        {
            LogicTest();
            //TestWorkflow();


        }


    static public void TestWorkflow()
        {
            ActivityXamlServicesSettings settings = new ActivityXamlServicesSettings
            {
                CompileExpressions = true
            };

            Activity workflow = ActivityXamlServices.Load("TestFormActivity.xaml", settings);
            WorkflowInvoker.Invoke(workflow);

            Console.WriteLine("Press <enter> to exit");
            Console.ReadLine();
        }


        static void LogicTest()
        {
            Grid mainGrid = FormsCreator.GetGridFromFile("HelperFiles\\ParentForm.xaml");
            ResourceDictionary mainDictionary = FormsCreator.GetResourceDictionaryFromFile("HelperFiles\\DemoDictionary.xaml");


            //initialize input for the dictionary
            Dictionary<string, Dictionary<string, object>> input = new Dictionary<string, Dictionary<string, object>> {
                //{ "ComboBox2", new Dictionary<string, object>{ { "IsSelected", true } } },
                { "cmbBudgetYear", new Dictionary<string, object>{ { "SelectedValuePath", "Content" },
                                                                    { "SelectedValue", "2011" } } },
                { "test123", new Dictionary<string, object>{ { "SelectedValue", "ComboBox Item #2" },
                                                                    { "SelectedValuePath", "Text" } } },
                { "nameInput", new Dictionary<string, object>{ { "Text", "blabla" } } },
                { "McCheckBox", new Dictionary<string, object>{ { "IsChecked", true } } },
                { "testCheckBox", new Dictionary<string, object>{ { "IsChecked", true } } },
                { "tatae", new Dictionary<string, object>{ { "IsChecked", true } } },
                { "namerino123", new Dictionary<string,object>{ { "Text", "Papa Don't Preach \n lalalaalalla" } } }//,
                
            };


            Dictionary<string, Dictionary<string, object>> input1 = new Dictionary<string, Dictionary<string, object>> {
                //{ "ComboBox2", new Dictionary<string, object>{ { "IsSelected", true } } },
                { "tb1", new Dictionary<string, object>{ { "Text", "blabla" } } },
                { "tb2", new Dictionary<string, object>{ { "Text", "blabla2" } } },
                { "tb3", new Dictionary<string, object>{ { "Text", "blabla3" } } }

            };

            Dictionary<string, Dictionary<string, object>> input2 = new Dictionary<string, Dictionary<string, object>> {
                //{ "ComboBox2", new Dictionary<string, object>{ { "IsSelected", true } } },
                 { "employee", new Dictionary<string, object>{ { "Source", new BitmapImage(new Uri("C:\\UiPath\\CustomActivities\\Community.Activities\\WpfFormCreator\\UiPath.Team.WPFFormCreator.TestProject\\TestFiles\\albertcamus.jpg", UriKind.Absolute)) } } },
                 { "hireDate", new Dictionary<string, object>{ { "Value", DateTime.Now} } },
                { "firstName", new Dictionary<string, object>{ { "Text", "Albert" } } }

            };

            Dictionary<string, Dictionary<string, object>> Results;
            Results =
              FormsCreator.LaunchForm(
              //"TestFiles\\ParentForm_6.xaml",
              "C:\\Users\\raduBucur\\Documents\\UiPath\\testWPFFormCreator\\Example2\\TestFiles\\ParentForm_6.xml",
              "HelperFiles\\DemoDictionary.xaml",
              "submitButton", "Click",
              input2,
              false);

            /*Results =
                FormsCreator.LaunchForm(
                "TestFiles\\ParentForm.xaml",
                "HelperFiles\\DemoDictionary.xaml",
                "tatae", "Click",
                input,

                true);*/


            /*Results =
               FormsCreator.LaunchForm(
               "HelperFiles\\ParentForm_2.xaml",
               "HelperFiles\\DemoDictionary.xaml",
               "buttonClickClick", "Click",
               input1,
               false);*/

            Console.WriteLine(Results.Count + String.Join (Environment.NewLine,
                Results.Select(x => x.Key + ":" + Environment.NewLine + String.Join(";" + Environment.NewLine, x.Value.Select(
                        y => y.Key + "->" + (y.Value == null?"":y.Value.ToString()))) + Environment.NewLine)) );

            Console.ReadLine();
            /*
            //initialize execution
            //CustomFormWindow customWindow = new CustomFormWindow(mainGrid, mainDictionary,"Button", "testButton", "Click");
            CustomFormWindow customWindow = new CustomFormWindow(mainGrid, mainDictionary, "tatae", "Click", input);
            customWindow.ShowDialog();

            Dictionary<string, Dictionary<string, object>> Results = customWindow.Results;
            */
        }

        [STAThread]
        static void Main123()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Button Name=\"testButton\" Width=\"100\" Height=\"30\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">Button</Button>");

            string jsonText = JsonConvert.SerializeXmlNode(doc);
            Console.WriteLine(jsonText);


            Thread.CurrentThread.SetApartmentState(System.Threading.ApartmentState.STA);
            System.Windows.Controls.Button wpfButton = null;


            using (FileStream fs = new FileStream("JSON.txt", FileMode.Open, FileAccess.Read))

            {

                string json =(new StreamReader(fs)).ReadToEnd();

                XNode node = JsonConvert.DeserializeXNode(json, "Button");
                Console.WriteLine(node.ToString());

                //converting the node to string so we convert it to wpf elem
                Stream s = GenerateStreamFromString(node.ToString());

                // Load WPF button with XamlReader
                wpfButton = (System.Windows.Controls.Button)XamlReader.Load(s);
            }



            

            // Create a Windows Form and show it

            using (Form wpfHostForm = new Form())

            {

                // Use ElementHost control to host a WPF element
                ElementHost wpfElementHost = new ElementHost();
                wpfElementHost.Dock = DockStyle.Fill;
                wpfHostForm.Controls.Add(wpfElementHost);
                wpfElementHost.Child = wpfButton;
                //wpfButton.Click += button_Click;
                mainForm = wpfHostForm;
                wpfHostForm.ShowDialog();
            }


            
        }


        static private DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                Console.WriteLine("Elem: " + tmp.GetType().ToString());
                parent = tmp;
            }
            return parent;
        }


        static private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}


/*

XmlDocument doc = new XmlDocument();
doc.LoadXml("<Button Name=\"testButton\" Width=\"100\" Height=\"30\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">Button</Button>");

string jsonText = JsonConvert.SerializeXmlNode(doc);
Console.WriteLine(jsonText); 
     
*/
