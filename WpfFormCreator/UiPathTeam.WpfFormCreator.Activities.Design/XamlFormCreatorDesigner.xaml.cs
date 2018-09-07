using Microsoft.Win32;
using System.Activities;
using System.Activities.Presentation.Model;
using System.IO;
using System.Windows;

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
             _openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            _openFileDialog.Filter = "Excel Files|*.xml;*.xaml";


            if (_openFileDialog.ShowDialog() == true)
            {
                ModelProperty property = this.ModelItem.Properties["StyleSheetPath"];
                property.SetValue(new InArgument<string>(Utils.TrimFilePath(_openFileDialog.FileName, Directory.GetCurrentDirectory())));

            }
        }

        private void Button_Click_FormXAMLPath(object sender, RoutedEventArgs e)
        {

            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Title = "Open Xaml File";
            _openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            _openFileDialog.Filter = "Excel Files|*.xml;*.xaml";

            if (_openFileDialog.ShowDialog() == true)
            {
                ModelProperty property = this.ModelItem.Properties["FormXAMLPath"];
                property.SetValue(new InArgument<string>(Utils.TrimFilePath(_openFileDialog.FileName, Directory.GetCurrentDirectory())));
            }
        }
    }
}
