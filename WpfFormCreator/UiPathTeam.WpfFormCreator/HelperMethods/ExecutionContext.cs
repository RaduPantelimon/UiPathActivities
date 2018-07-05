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

namespace UiPathTeam.WpfFormCreator.HelperMethods
{
    public class ExecutionContext
    {
        public string contentPath;
        public string stylePath;
        public string submitElementName;
        public string submitEventName;
        public string[] elementsToRetrieve;
        public Dictionary<string, Dictionary<string, object>> input;
        public bool getAllProperties;

        public Grid MainElement;
        public ResourceDictionary MainDictionary;

        public ExecutionContext(string _contentPath,
                                string _stylePath,
                                string _submitElementName,
                                string _submitEventName,
                                string[] _elementsToRetrieve,
                                Dictionary<string, Dictionary<string, object>> _input,
                                bool _getAllProperties)
        {

           
            if ((_elementsToRetrieve == null || _elementsToRetrieve.Length == 0) &&
                (_input == null || _input.Count ==0 ))
            {
                throw new Exception(WpfFormCreatorResources.ErrorMessage_EmptyInput);
            }

            contentPath = _contentPath;
            stylePath = _stylePath;
            submitElementName = _submitElementName;
            submitEventName = _submitEventName;
            getAllProperties = _getAllProperties;
            input = _input;
            elementsToRetrieve = _elementsToRetrieve;


            MainElement = FormsCreator.GetGridFromFile(contentPath);
            if (!String.IsNullOrEmpty(_stylePath)) MainDictionary = FormsCreator.GetResourceDictionaryFromFile(stylePath);
        }

    }
}
