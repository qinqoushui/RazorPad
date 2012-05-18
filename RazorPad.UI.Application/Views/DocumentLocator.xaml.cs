using System.ComponentModel.Composition;
using System.Windows;
using RazorPad.Persistence;

namespace RazorPad.Views
{
    // Un-comment to include
    //[Export(typeof(IRazorDocumentLocator))]
    public partial class DocumentLocator : Window, IRazorDocumentLocator
    {
        public DocumentLocator()
        {
            InitializeComponent();
        }

        public string Locate()
        {
            if (ShowDialog() == true)
                return Location.Text;
            else
                return null;
        }
    }
}
