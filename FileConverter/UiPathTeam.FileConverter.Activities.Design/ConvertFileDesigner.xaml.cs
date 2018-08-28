using System.Activities;
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace UiPathTeam.FileConverter.Activities.Design
{
    // Interaction logic for ConvertXLSToXLSX.xaml
    public partial class ConvertFileDesigner
    {
        public ConvertFileDesigner()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Creates a file selection pop-up and allows the user to select the file to convert
        /// </summary> 
        private void Button_Click_SelectFile(object sender, RoutedEventArgs e)
        {
            //selecting the file to convert
            Microsoft.Win32.OpenFileDialog _openFileDialog = new Microsoft.Win32.OpenFileDialog();
            _openFileDialog.Title = "Select File";
            _openFileDialog.Filter = this.ModelItem.Properties["OldFileExtension"].ComputedValue.ToString();
            _openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();


            if (_openFileDialog.ShowDialog() == true)
            {
                ModelProperty property = this.ModelItem.Properties["OldFilePath"];
                //if the selected folder is inside the current directory, we will trim the start of the name
                property.SetValue(new InArgument<string>(Utils.TrimFilePath(_openFileDialog.FileName, Directory.GetCurrentDirectory())));

            }
        }




        /// <summary>
        /// Creates a folder selection pop-up and allows the user to  select the directory in which to save the converted file
        /// </summary> 
        private void Button_Click_SelectFolder(object sender, RoutedEventArgs e)
        {

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Directory.GetCurrentDirectory();
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    ModelProperty property = this.ModelItem.Properties["DirectoryToSave"];
                    //if the selected folder is inside the current directory, we will trim the start of the name
                    property.SetValue(new InArgument<string>(Utils.TrimFilePath(fbd.SelectedPath, Directory.GetCurrentDirectory())));
                }
            }
        }
    }
}
