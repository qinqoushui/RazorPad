using System.Windows.Controls;
using System.Windows;
using RazorPad.ViewModels;

namespace RazorPad.Views
{
    class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DocumentEditorTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is RazorTemplateEditorViewModel)
                return DocumentEditorTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
