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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibraryMetadata;
namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filename;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void OpenLibraryCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "AssemblyMetadata"; // Default file name
            dialog.DefaultExt = ".dll"; // Default file extension
            dialog.Filter = "Libraries (.dll)|*.dll"; // Filter files by extension

            // Show open file dialog box    
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                _filename = dialog.FileName;
                ExtractInfo();
            }
        }
        private void ExtractInfo()
        {
            Tree.Items.Clear();
            if (String.IsNullOrEmpty(_filename))
                return;
            IMetadataInformer metadataInformer = new MetadataInformer();
            var treeViewItems = metadataInformer.GetTreeMetaData(_filename);
            foreach (var treeItem in treeViewItems)
            {
                Tree.Items.Add(treeItem);
            }
            //Tree.UpdateLayout();
        }
        
    }
}
