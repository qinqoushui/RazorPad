using System;
using System.Windows;
using System.Windows.Forms;
using RazorPad.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace RazorPad.Views
{
    public partial class ReferencesDialogWindow
    {
        protected ReferencesViewModel ViewModel
        {
            get { return DataContext as ReferencesViewModel; }
        }

        public ReferencesDialogWindow()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".dll",
                Filter = "Component Files (.dll)|*.dll"
            };

            var result = ofd.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) 
                return;

            if (ViewModel == null) 
                return;

            string message;
            foreach (var filePath in ofd.FileNames)
            {
                try
                {
                    var referenceAdded = ViewModel.TryAddReference(filePath, out message);

                    if (!referenceAdded)
                    {
                        MessageBox.Show(
                            "Could not add reference due to: " + message, 
                            "Add Reference Error", 
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                catch (ArgumentException aex)
                {
                    MessageBox.Show(
                        "Could not add reference due to: " + aex.Message, 
                        "Add Reference Error", 
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
