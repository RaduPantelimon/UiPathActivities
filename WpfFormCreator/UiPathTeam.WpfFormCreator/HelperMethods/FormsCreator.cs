using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using System.IO;
using Xceed.Wpf.Toolkit;

using UiPathTeam.WpfFormCreator.HelperMethods;
using System.Windows.Markup;
using System.Reflection;

namespace UiPathTeam.WpfFormCreator
{
    public static partial class FormsCreator
    {



        /// <summary>
        /// Launches the form and returns the output values after the form has been closed
        /// </summary>
        public static Dictionary<string, Dictionary<string, object>> LaunchForm(
            string contentPath,
            string stylePath,
            string submitElementName,
            string submitEventName,
            Dictionary<string, Dictionary<string, object>> input,
            bool alwaysTop,
            bool getAllElements = false,
            string[] elementsToRetrieve = null)
        {

            //instaintiate execution context
            ExecutionContext executionContext = new ExecutionContext(contentPath,
                                 stylePath,
                                 submitElementName,
                                 submitEventName,
                                 elementsToRetrieve,
                                 input,
                                 getAllElements,
                                 alwaysTop);

            //show window
            CustomFormWindow customWindow = new CustomFormWindow(executionContext);
            customWindow.ShowDialog();

            //get results
            Dictionary<string, Dictionary<string, object>> Results = customWindow.Results;

            return Results;
        }

        /// <summary>
        /// Loads up a resource dictionary
        /// </summary>
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


        /// <summary>
        /// Gets the a grid element from a xaml file, this will be the grid where we will store everything
        /// </summary>
        public static Grid GetGridFromFile(string filePath)
        {
            try
            {

                //load file
                XDocument mainComponent = XDocument.Load(filePath);
                XElement mainAppElem = XElement.Parse(mainComponent.FirstNode.ToString());

                //converting the node to string so we convert it to wpf elem
                Stream s = Utils.GenerateStreamFromString(mainAppElem.ToString());
                DateTimePicker dtp = new DateTimePicker();
                dtp.Value = DateTime.Now;

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


        /// <summary>
        /// finds the child element with a specific name inside the current WPF element
        /// </summary>
        public static FrameworkElement FindChild(DependencyObject parent, string childName)
        {
            // Confirm parent and childName are valid.   
            if (parent == null) return null;

            FrameworkElement foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var frameworkElement = child as FrameworkElement;
                // If the child's name is set for search  
                if (frameworkElement != null && (frameworkElement.Name == childName))
                {
                    // if the child's name is of the request name  
                    foundChild = frameworkElement;
                    break;
                }
                else
                {
                    foundChild = FindChild(child, childName);
                    if (foundChild != null) break;
                }

               
                Control childAsControl = child as Control;
                //child
                if (childAsControl == null)
                {
                    // recursively drill down the tree  
                    foundChild = FindChild(child, childName);

                    // If the child is found, break so we do not overwrite the found child.   
                    if (foundChild != null) break;
                }

              
            }

            return foundChild; 
        }


        /// <summary>
        /// finds the children elements with specific names inside the current WPF element
        /// </summary>
        public static List<FrameworkElement> FindChildren(DependencyObject parent, string[] childrenNames)
        {
            // Confirm parent and childName are valid.   
            if (parent == null) return null;

            List<FrameworkElement> foundChildren = new List<FrameworkElement>();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                FrameworkElement childAsControl = child as FrameworkElement;

                var frameworkElement = child as FrameworkElement;
                // If the child's name is set for search  

                //save all elements that have a pre-specified name
                if (childAsControl != null && frameworkElement != null && (childrenNames.Contains(frameworkElement.Name)) )
                {
                    // if the child's name is of the request name  
                    foundChildren.Add(childAsControl);

                }
                foundChildren.AddRange(FindChildren(child, childrenNames/*, saveEverything*/));

            }

            return foundChildren;
        }



        /// <summary>
        /// sets a value for a named property of a WPF element
        /// </summary>
        public static void SetValueForProperty(FrameworkElement wpfFrameworkElement, string propertyName, object propertyValue)
        {
            //get property name
            PropertyInfo propertyInfo = wpfFrameworkElement.GetType().GetProperty(propertyName);
            //set value
            try
            {
                propertyInfo.SetValue(wpfFrameworkElement, propertyValue, null);
            }
            catch(Exception ex)
            {
                throw new Exception(String.Format(WpfFormCreatorResources.ErrorMessage_ValueTypeInvalid, wpfFrameworkElement.Name, propertyName,
                   ex.ToString()));
            }
           
        }

        public static object GetValueForProperty(FrameworkElement wpfControl, string propertyName)
        {
            return wpfControl.GetType().GetProperty(propertyName).GetValue(wpfControl, null);
        }


        public static Dictionary<string, Dictionary<string, object>> GetDataFromWPFWindow(
            FrameworkElement[] controlsToParseForOutput, 
            bool getAllData = false,
            Dictionary<string, Dictionary<string, object>> Inputs = null)
        {
            Dictionary<string, Dictionary<string, object>> Results = new Dictionary<string, Dictionary<string, object>>();

            //loop through the Elements in the dictionary
            foreach (FrameworkElement currentFrameworkElement in controlsToParseForOutput)
            {
                if(!getAllData)
                {
                    //get the properties mapped in the Input Dictionary for the current Control 
                    KeyValuePair<string, Dictionary<string, object>> elementValuesPair = Inputs.Where(x => x.Key == currentFrameworkElement.Name).FirstOrDefault();
                    Dictionary<string, object> resultsForControl = FormsCreator.GetPropertiesFromControl(currentFrameworkElement, elementValuesPair.Value);
                    Results.Add(elementValuesPair.Key, resultsForControl);
                }
                else
                {
                    //get all non-null properties from the Control
                    Dictionary<string, object> resultsForControl = FormsCreator.GetAllPropertiesFromControl(currentFrameworkElement);
                    Results.Add(currentFrameworkElement.Name, resultsForControl);
                }
                

            }

            return Results;
        }


        /// <summary>
        /// gets all the properties of a WPF control
        /// </summary>
        public static Dictionary<string, object> GetAllPropertiesFromControl(FrameworkElement currentFrameWorkElement)
        {
            Dictionary<string, object> resultsForControl = new Dictionary<string, object>();

            //loop through all properties and if the value is not null, we store it in the dictionary
            foreach (PropertyInfo pi in currentFrameWorkElement.GetType().GetProperties())
            {
               
                    object value = pi.GetValue(currentFrameWorkElement);
                    if (value != null && !String.IsNullOrEmpty(value.ToString()) )
                    {
                        resultsForControl.Add(pi.Name, value);
                    }
            }

            return resultsForControl;
        }


        /// <summary>
        /// gets a named property of a WPF control
        /// </summary>
        public static Dictionary<string,object> GetPropertiesFromControl(FrameworkElement currentFrameworkElement, Dictionary<string,object> propertiesToGet)
       {
            //loop through all properties in the input dictionary and create an output dictionary
            Dictionary<string, object> resultsForControl = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> nameValuePair in propertiesToGet)
            {
                //get value and add it to the dictionary
                object valueObtained = FormsCreator.GetValueForProperty(currentFrameworkElement, nameValuePair.Key);
                resultsForControl.Add(nameValuePair.Key, valueObtained);
            }

            return resultsForControl;
       }


        /// <summary>
        /// changes a nullable object to a non-nullable type
        /// </summary>
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
