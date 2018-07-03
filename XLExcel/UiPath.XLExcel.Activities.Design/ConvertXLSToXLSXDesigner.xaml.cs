using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UiPathTeam.XLExcel.Activities.Design
{
    // Interaction logic for ConvertXLSToXLSX.xaml
    public partial class ConvertXLSToXLSXDesigner
    {
        public ConvertXLSToXLSXDesigner()
        {
            InitializeComponent();
        }


        private void Button_Click_SelectFile(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog _openFileDialog = new Microsoft.Win32.OpenFileDialog();
            _openFileDialog.Title = "Open XLS File";
            _openFileDialog.Filter = "Excel Files|*.xls";
            _openFileDialog.InitialDirectory = @"C:\";

            if (_openFileDialog.ShowDialog() == true)
            {
                ModelProperty property = this.ModelItem.Properties["OldFilePath"];
                //property
                property.SetValue(new InArgument<string>(_openFileDialog.FileName));

            }
        }


        private void Button_Click_SelectFolder(object sender, RoutedEventArgs e)
        {

            using (var fbd = new FolderBrowserDialog())
            {
               
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    ModelProperty property = this.ModelItem.Properties["DirectoryToSave"];
                    //property
                    property.SetValue(new InArgument<string>(fbd.SelectedPath));
                }
            }
        }
    }
}
