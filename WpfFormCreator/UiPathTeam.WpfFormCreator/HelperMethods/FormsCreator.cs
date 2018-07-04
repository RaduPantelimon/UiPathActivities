using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml;
using System.IO;


using UiPathTeam.WpfFormCreator.HelperMethods;
using System.Windows.Markup;
using System.Reflection;

namespace UiPathTeam.WpfFormCreator
{
    public static partial class FormsCreator
    {

        public static Dictionary<string, Dictionary<string, object>> LaunchForm(
            string contentPath,
            string stylePath,
            string submitElementName,
            string submitEventName,
            Dictionary<string, Dictionary<string, object>> input,
            bool getAllElements = false, 
            string[] elementsToRetrieve = null
            )
        {


            ExecutionContext executionContext = new ExecutionContext(contentPath,
                                 stylePath,
                                 submitElementName,
                                 submitEventName,
                                 elementsToRetrieve,
                                 input,
                                 getAllElements);

            CustomFormWindow customWindow = new CustomFormWindow(executionContext);
            customWindow.ShowDialog();

            Dictionary<string, Dictionary<string, object>> Results = customWindow.Results;

            return Results;
        }


        public static ResourceDictionary  GetResourceDictionaryFromFile(string filePath)
        {
            try
            {
                //load file
                XDocument resDictionaryDocument = XDocument.Load(filePath);
                XElement mainResDictionary = XElement.Parse(resDictionaryDocument.FirstNode.ToString());

                // Load WPF Grid with XamlReader
                Stream stream = Utils.GenerateStreamFromString(mainResDictionary.ToString());
                ResourceDictionary myResourceDictionary = (ResourceDictionary)XamlReader.Load(stream);

                return myResourceDictionary;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(WpfFormCreatorResources.ErrorMessage_InvaidXAMLStyleSheet,
                    ex.ToString()));
            }
        }

        public static Grid GetGridFromFile(string filePath)
        {
            try
            {

                //load file
                XDocument mainComponent = XDocument.Load(filePath);
                XElement mainAppElem = XElement.Parse(mainComponent.FirstNode.ToString());

                //converting the node to string so we convert it to wpf elem
                Stream s = Utils.GenerateStreamFromString(mainAppElem.ToString());

                // Load WPF Grid with XamlReader
                System.Windows.Controls.Grid mainGrid = null;
                mainGrid = (System.Windows.Controls.Grid)XamlReader.Load(s);
                return mainGrid;
            }
            catch(Exception ex)
            {
                throw new Exception(String.Format(WpfFormCreatorResources.ErrorMessage_InvalidFormXAML,
                    ex.ToString()));
            }
            
            
        }

        public static Control FindChild(DependencyObject parent, string childName)
        {
            // Confirm parent and childName are valid.   
            if (parent == null) return null;

            Control foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                Control childAsControl = child as Control;

                //child
                if (childAsControl == null)
                {
                    // recursively drill down the tree  
                    foundChild = FindChild(child, childName);

                    // If the child is found, break so we do not overwrite the found child.   
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search  
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name  
                        foundChild = (Control)child;
                        break;
                    }
                    else
                    {
                        foundChild = FindChild(child, childName);
                        //break;
                    }
                }
                else
                {
                    // child element found.  
                    foundChild = (Control)child;
                    break;
                }
            }

            return foundChild; 
        }


        public static List<Control> FindChildren(DependencyObject parent, string[] childrenNames/*, bool saveEverything = false*/)
        {
            // Confirm parent and childName are valid.   
            if (parent == null) return null;

            List<Control> foundChildren = new List<Control>();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                Control childAsControl = child as Control;

                var frameworkElement = child as FrameworkElement;
                // If the child's name is set for search  

                //save all elements that have a pre-specified name
                if (childAsControl != null && frameworkElement != null && (childrenNames.Contains(frameworkElement.Name)) // ||
                  /* (saveEverything && !String.IsNullOrEmpty(frameworkElement.Name.Trim())) )*/)
                {
                    // if the child's name is of the request name  
                    foundChildren.Add(childAsControl);

                }
                foundChildren.AddRange(FindChildren(child, childrenNames/*, saveEverything*/));

            }

            return foundChildren;
        }

        public static void SetValueForProperty(Control WPFControl, string propertyName, object propertyValue)
        {
            //get property name
            PropertyInfo propertyInfo = WPFControl.GetType().GetProperty(propertyName);
            //set value
            propertyInfo.SetValue(WPFControl, ChangeType(propertyValue, propertyInfo.PropertyType), null);
        }

        public static object GetValueForProperty(Control WPFControl, string propertyName)
        {
            return WPFControl.GetType().GetProperty(propertyName).GetValue(WPFControl, null);
        }


        public static Dictionary<string, Dictionary<string, object>> GetDataFromWPFWindow(
            Control[] controlsToParseForOutput, 
            bool GetAllData = false,
            Dictionary<string, Dictionary<string, object>> Inputs = null)
        {
            Dictionary<string, Dictionary<string, object>> Results = new Dictionary<string, Dictionary<string, object>>();

            //loop through the Elements in the dictionary
            foreach (Control currentControl in controlsToParseForOutput)
            {
                if(!GetAllData)
                {
                    //get the properties mapped in the Input Dictionary for the current Control 
                    KeyValuePair<string, Dictionary<string, object>> elementValuesPair = Inputs.Where(x => x.Key == currentControl.Name).FirstOrDefault();
                    Dictionary<string, object> resultsForControl = FormsCreator.GetPropertiesFromControl(currentControl, elementValuesPair.Value);
                    Results.Add(elementValuesPair.Key, resultsForControl);
                }
                else
                {
                    //get all non-null properties from the Control
                    Dictionary<string, object> resultsForControl = FormsCreator.GetAllPropertiesFromControl(currentControl);
                    Results.Add(currentControl.Name, resultsForControl);
                }
                

            }

            return Results;
        }

        public static Dictionary<string, object> GetAllPropertiesFromControl(Control currentControl)
        {
            Dictionary<string, object> resultsForControl = new Dictionary<string, object>();

            //loop through all properties and if the value is not null, we store it in the dictionary
            foreach (PropertyInfo pi in currentControl.GetType().GetProperties())
            {
               
                    object value = pi.GetValue(currentControl);
                    if (value != null && !String.IsNullOrEmpty(value.ToString()) )
                    {
                        resultsForControl.Add(pi.Name, value);
                    }
            }

            return resultsForControl;
        }

       public static Dictionary<string,object> GetPropertiesFromControl(Control currentControl, Dictionary<string,object> propertiesToGet)
       {
            //loop through all properties in the input dictionary and create an output dictionary
            Dictionary<string, object> resultsForControl = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> nameValuePair in propertiesToGet)
            {
                //get value and add it to the dictionary
                object valueObtained = FormsCreator.GetValueForProperty(currentControl, nameValuePair.Key);
                resultsForControl.Add(nameValuePair.Key, valueObtained);
            }

            return resultsForControl;
       }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }



    }
}


/*
   public static Grid GetGridFromFile(string filePath)
        {
            //load file
            XDocument mainComponent = XDocument.Load(filePath);

            XNamespace aw = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XElement mainAppElem = new XElement(aw + "Grid", 
                new XAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"),
                new XAttribute(XNamespace.Xmlns + "x", "http://schemas.microsoft.com/winfx/2006/xaml"),
                new XAttribute(XNamespace.Xmlns + "d", "http://schemas.microsoft.com/expression/blend/2008"),
                new XAttribute(XNamespace.Xmlns + "mc", "http://schemas.openxmlformats.org/markup-compatibility/2006"),
                XElement.Parse(mainComponent.FirstNode.ToString()));


            //converting the node to string so we convert it to wpf elem
            Stream s = Utils.GenerateStreamFromString(mainAppElem.ToString());

            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
            context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");


            // Load WPF Grid with XamlReader
            System.Windows.Controls.Grid mainGrid = null;
            mainGrid = (System.Windows.Controls.Grid)XamlReader.Load(s, context);

            return mainGrid;
        }
     
     
     */
