using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UiPathTeam.WpfFormCreator.Activities.Design
{
    // Interaction logic for XamlFormCreatorDesigner.xaml
    public partial class XamlFormCreatorDesigner
    {
        public XamlFormCreatorDesigner()
        {
            InitializeComponent();
        }

        private void Button_Click_StyleSheetPath(object sender, RoutedEventArgs e)
        {

            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Title = "Open Xaml File";
            _openFileDialog.Filter = "Excel Files|*.xml;*.xaml";
            _openFileDialog.InitialDirectory = @"C:\";

            if (_openFileDialog.ShowDialog() == true)
            {
                ModelProperty property = this.ModelItem.Properties["StyleSheetPath"];
                //property
                property.SetValue(new InArgument<string>(_openFileDialog.FileName));

            }
        }

        private void Button_Click_FormXAMLPath(object sender, RoutedEventArgs e)
        {

            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Title = "Open Xaml File";
            _openFileDialog.Filter = "Excel Files|*.xml;*.xaml";
            _openFileDialog.InitialDirectory = @"C:\";

            if (_openFileDialog.ShowDialog() == true)
            {
                ModelProperty property = this.ModelItem.Properties["FormXAMLPath"];
                //property
                property.SetValue(new InArgument<string>(_openFileDialog.FileName));

            }
        }
    }
}
