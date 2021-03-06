﻿using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using System.Activities.Validation;

namespace UiPathTeam.WpfFormCreator.Activities
{
    public class XamlFormCreator:NativeActivity
    {

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("XAML Form Path")]
        [Description("Path to the file containing the XAML Markup used in rendering the form")]
        public InArgument<string> FormXAMLPath { get; set; }

        [Category("Input")]
        [Description("Path to the file containing the Resource Dictionary used in styling the form")]
        [DisplayName("StyleSheet Path")]
        public InArgument<string> StyleSheetPath { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Submit Element Name")]
        [Description("Name of the Element on which we perform the submit event")]
        public InArgument<string> SubmitElementName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Submit Event Name")]
        [Description("Name of the Event on which we perform the submit(e.g. click)")]
        public InArgument<string> SubmitEventName { get; set; }

        [Category("Input")]
        [DisplayName("Inputs Dictionary")]
        [Description("Initialization values for all the controls in the form. By default, this also describes the properties which will be retrieved at submit")]
        public InArgument<Dictionary<string, Dictionary<string, object>>> InputDictionary { get; set; }

        [Category("Input")]
        [DisplayName("Get All Properties")]
        [Description("If this is checked, the submit event will return all non-null properties of all the controls mentioned in the Elements To Retrieve array")]
        public bool GetAllProperties { get; set; }

        [Category("Input")]
        [DisplayName("Elements To Retrieve")]
        [Description("Array with the names of the controls for which, at submit, we should retrieve the all the non-null properties")]
        public InArgument<string[]> ElementsToRetrieve { get; set; }

        [Category("Input")]
        [DisplayName("Always Top")]
        [Description("If this is checked, the form will always appear in the foreground until it is submitted")]
        public bool AlwaysTop { get; set; }

        [Category("Output")]
        [Description("The values returned when the form is submitted")]
        public OutArgument<Dictionary<string, Dictionary<string, object>>> OutputDictionary { get; set; }


        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {

            
            //performing validation checks
            if(GetAllProperties == false && InputDictionary == null)
            {
                ValidationError error = new ValidationError(WpfFormCreatorResources.ErrorMessage_NoInputGiven);
                metadata.AddValidationError(error);
            }

            if(GetAllProperties == true && ElementsToRetrieve == null)
            {
                ValidationError error = new ValidationError(WpfFormCreatorResources.ErrorMessage_ElementsToRetrieveMissing);
                metadata.AddValidationError(error);
            }
            if (GetAllProperties == false && ElementsToRetrieve != null)
            {
                ValidationError error = new ValidationError(WpfFormCreatorResources.ErrorMessage_ElementsToRetrieveError);
                metadata.AddValidationError(error);
            }

            base.CacheMetadata(metadata);

        }

        protected override void Execute(NativeActivityContext context)
        {
            //get data from context
            bool getAllProperties = GetAllProperties;
            string formXAMLPath = FormXAMLPath.Get(context);
            string styleSheetPath = StyleSheetPath.Get(context);
            string submitElementName = SubmitElementName.Get(context);
            string submitEventName = SubmitEventName.Get(context);
            Dictionary<string, Dictionary<string, object>> input = InputDictionary.Get(context);
            string[] elementsToRetrieve = ElementsToRetrieve.Get(context);


            //launch form and get result data
            Dictionary<string, Dictionary<string, object>> results = FormsCreator.LaunchForm(
               formXAMLPath,
               styleSheetPath,
               submitElementName, 
               submitEventName,
               input,
               AlwaysTop,
               GetAllProperties,
               elementsToRetrieve
               );

            //set output value
            OutputDictionary.Set(context,results);
        }
    }
}
