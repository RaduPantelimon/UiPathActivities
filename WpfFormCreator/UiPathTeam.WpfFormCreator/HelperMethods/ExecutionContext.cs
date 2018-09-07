using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UiPathTeam.WpfFormCreator.HelperMethods
{


    /// <summary>
    /// Class that contains all the necessary information with regards to running this workflow
    /// </summary>
    public class ExecutionContext
    {
        public string ContentPath;
        public string StylePath;
        public string SubmitElementName;
        public string SubmitEventName;
        public string[] ElementsToRetrieve;
        public Dictionary<string, Dictionary<string, object>> Input;
        public bool GetAllProperties;
        public bool TopMost;


        public Grid MainElement;
        public ResourceDictionary MainDictionary;

        public ExecutionContext(string contentPath,
                                string stylePath,
                                string submitElementName,
                                string submitEventName,
                                string[] elementsToRetrieve,
                                Dictionary<string, Dictionary<string, object>> input,
                                bool getAllProperties,
                                bool topMost = false)
        {

           
            if ((elementsToRetrieve == null || elementsToRetrieve.Length == 0) &&
                (input == null || input.Count ==0 ))
            {
                throw new Exception(WpfFormCreatorResources.ErrorMessage_EmptyInput);
            }

            ContentPath = contentPath;
            StylePath = stylePath;
            SubmitElementName = submitElementName;
            SubmitEventName = submitEventName;
            GetAllProperties = getAllProperties;
            Input = input;
            ElementsToRetrieve = elementsToRetrieve;
            TopMost = topMost;

            MainElement = FormsCreator.GetGridFromFile(ContentPath);
            if (!String.IsNullOrEmpty(stylePath)) MainDictionary = FormsCreator.GetResourceDictionaryFromFile(StylePath);
        }

    }
}
