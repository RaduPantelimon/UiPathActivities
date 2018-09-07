using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using UiPathTeam.WpfFormCreator.HelperMethods;
using System.Reflection;

namespace UiPathTeam.WpfFormCreator
{
    /// <summary>
    /// Interaction logic for CustomFormWindow.xaml
    /// </summary>
    /// 
    public delegate void EventDelegate(object sender, RoutedEventArgs e);

    public partial class CustomFormWindow : Window
    {

        public Dictionary<string, Dictionary<string, object>> Inputs;
        public Dictionary<string, Dictionary<string, object>> Results;
        public bool SaveEverything;
        public bool AlwaysTop;
        public string[] ControlsToSave;


        public CustomFormWindow()
        {
            InitializeComponent();
        }

        public CustomFormWindow(Grid mainElem, 
            ResourceDictionary res, 
            string ElementName, 
            string Event, 
            Dictionary<string, Dictionary<string, object>> initializationValues,
            bool _saveEverithing)
        {
            InitializeComponent();

            //load additional resources provided by the user
            if (res != null) this.Resources.MergedDictionaries.Add(res);

            //initialize the initial input dictionary
            Inputs = initializationValues;

            //add the main form body
            StackPanel.Children.Add(mainElem);

            //initialize the values for the controls inside the form
            SetInitializationValues(initializationValues);

            //set submit click on the specified event of the specified item
            SetEvent(ElementName, Event, CloseAndSaveResults);

            SaveEverything =_saveEverithing;
            
        }

        public CustomFormWindow(ExecutionContext executionContext)
        {
            InitializeComponent();

            //bring the window to the foreground and activate it
            this.Activate();
            this.Focus();


            //load additional resources provided by the user
            if (executionContext.MainDictionary != null) this.Resources.MergedDictionaries.Add(executionContext.MainDictionary);

            //initialize the initial input dictionary
            Inputs = executionContext.Input;

            //add the main form body
            StackPanel.Children.Add(executionContext.MainElement);

            //initialize the values for the controls inside the form
            if (Inputs!= null) SetInitializationValues(Inputs);

            //set submit click on the specified event of the specified item
            SetEvent(executionContext.SubmitElementName, executionContext.SubmitEventName, CloseAndSaveResults);

            SaveEverything = executionContext.GetAllProperties;
            ControlsToSave = executionContext.ElementsToRetrieve;
            AlwaysTop = executionContext.TopMost;



        }

        private void SetEvent(string ElementName, string Event, RoutedEventHandler closeAndSaveResults)
        {
            //set the click event
            string assemblyfullName = typeof(Button).Assembly.FullName;
            //Type type = Type.GetType("System.Windows.Controls." + ElemType + ", " + assemblyfullName);

            MethodInfo method = typeof(FormsCreator).GetMethod("FindChild");
            //MethodInfo generic = method.MakeGenericMethod(type);
            //dynamic result = generic.Invoke(this, new object[] { StackPanel, ElementName });
            dynamic result = method.Invoke(this, new object[] { StackPanel, ElementName });

            Type type = result.GetType();

            //set event
            if (result != null)
            {
                //get event and the AddMethod for it
                EventInfo evClick = type.GetEvent(Event);
                MethodInfo addHandler = evClick.GetAddMethod();

                //invoke AddMethod with the delegate we received as paramenter
                Object[] addHandlerArgs = { closeAndSaveResults };
                addHandler.Invoke(result, addHandlerArgs);
            }
        }



        /// <summary>
        /// Initializez the default values of all the WPF controls in the form
        /// </summary>
        /// <param name="initializationValues"></param>
        private void SetInitializationValues(Dictionary<string, Dictionary<string, object>> initializationValues)
        {
            //loop through the Elements in the dictionary
            foreach(KeyValuePair<string,Dictionary<string, object>> elementValuesPair in initializationValues)
            {
                //get element from DOM
                MethodInfo method = typeof(FormsCreator).GetMethod("FindChild");
                dynamic result = method.Invoke(this, new object[] { StackPanel, elementValuesPair.Key });
                if (result == null) throw new Exception("WPF control with name: " + elementValuesPair.Key + " was not found. Check the input dictionary Values");

                //set all properties for the current control
                foreach(KeyValuePair<string, object> nameValuePair in elementValuesPair.Value)
                {
                    FormsCreator.SetValueForProperty(result, nameValuePair.Key, nameValuePair.Value);
                }

            }
        }



        /// <summary>
        /// this is the event that is triggered at submit, it will collect all the input data and close the current form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAndSaveResults(object sender, RoutedEventArgs e)
        {
            
            
            //if we received a list of controls to save, we will 
            if(ControlsToSave!=null && ControlsToSave.Length > 0)
            {
                //scrape the Controls from the ControlsToSave name array
                List<FrameworkElement> controlsToParseForOutput = FormsCreator.FindChildren(StackPanel, ControlsToSave);
                Results = FormsCreator.GetDataFromWPFWindow(controlsToParseForOutput.ToArray(), SaveEverything, null);
            }
            else
            {
               //scrape the values provided for us in the Inputs table

                string[] elementNames;
                //get the element names from the input dictionary
                elementNames = Inputs.Select(x => x.Key).ToArray();
                List<FrameworkElement> controlsToParseForOutput = FormsCreator.FindChildren(StackPanel, elementNames);
                //scrape results
                Results = FormsCreator.GetDataFromWPFWindow(controlsToParseForOutput.ToArray(), SaveEverything, Inputs);
            }

            
            //close window
            Window.GetWindow(this).Close();
        }

        //override 

        //make window the topmost when initialized
        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Focus();
            
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //bring the window to the foreground and activate it
                this.Activate();
                this.Focus();

                if (AlwaysTop)
                {
                    //force the window to be always on top
                    this.Topmost = true;
                }
            }));
        }
    }
}

