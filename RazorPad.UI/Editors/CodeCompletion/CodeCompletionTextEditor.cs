using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;

namespace RazorPad.UI.Editors.CodeCompletion
{
    public class CodeCompletionTextEditor : TextEditor
    {
        //static CodeCompletionTextEditor()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeCompletionTextEditor),
        //                                             new FrameworkPropertyMetadata(typeof(CodeCompletionTextEditor)));
        //}


        public CodeCompletionTextEditor()
        {
			//AvalonEditDisplayBinding.RegisterAddInHighlightingDefinitions();
			
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, OnPrint));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.PrintPreview, OnPrintPreview));
			
			
		}
		
		protected virtual string FileName {
			get { return "untitled"; }
		}
		
		CodeCompletionWindow completionWindow;
		//SharpDevelopInsightWindow insightWindow;
		
		void CloseExistingCompletionWindow()
		{
			if (completionWindow != null) {
				completionWindow.Close();
			}
		}
		
        //void CloseExistingInsightWindow()
        //{
        //    if (insightWindow != null) {
        //        insightWindow.Close();
        //    }
        //}
		
		public CodeCompletionWindow ActiveCompletionWindow {
			get { return completionWindow; }
		}
		
        //public SharpDevelopInsightWindow ActiveInsightWindow {
        //    get { return insightWindow; }
        //}
		
		internal void ShowCompletionWindow(CodeCompletionWindow window)
		{
			CloseExistingCompletionWindow();
			completionWindow = window;
			window.Closed += delegate {
				completionWindow = null;
			};
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
				delegate {
					if (completionWindow == window) {
						window.Show();
					}
				}
			));
		}
		
        //internal void ShowInsightWindow(SharpDevelopInsightWindow window)
        //{
        //    CloseExistingInsightWindow();
        //    insightWindow = window;
        //    window.Closed += delegate {
        //        insightWindow = null;
        //    };
        //    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
        //        delegate {
        //            if (insightWindow == window) {
        //                window.Show();
        //            }
        //        }
        //    ));
        //}
		
		#region Printing
        //void OnPrint(object sender, ExecutedRoutedEventArgs e)
        //{
        //    PrintDialog printDialog = PrintPreviewViewContent.PrintDialog;
        //    if (printDialog.ShowDialog() == true) {
        //        FlowDocument fd = DocumentPrinter.CreateFlowDocumentForEditor(this);
        //        fd.ColumnGap = 0;
        //        fd.ColumnWidth = printDialog.PrintableAreaWidth;
        //        fd.PageHeight = printDialog.PrintableAreaHeight;
        //        fd.PageWidth = printDialog.PrintableAreaWidth;
        //        IDocumentPaginatorSource doc = fd;
        //        printDialog.PrintDocument(doc.DocumentPaginator, Path.GetFileName(this.FileName));
        //    }
        //}
		
        //void OnPrintPreview(object sender, ExecutedRoutedEventArgs e)
        //{
        //    PrintDialog printDialog = PrintPreviewViewContent.PrintDialog;
        //    FlowDocument fd = DocumentPrinter.CreateFlowDocumentForEditor(this);
        //    fd.ColumnGap = 0;
        //    fd.ColumnWidth = printDialog.PrintableAreaWidth;
        //    fd.PageHeight = printDialog.PrintableAreaHeight;
        //    fd.PageWidth = printDialog.PrintableAreaWidth;
        //    PrintPreviewViewContent.ShowDocument(fd, Path.GetFileName(this.FileName));
        //}
		#endregion
	}
	
    //sealed class ZoomLevelToTextFormattingModeConverter : IValueConverter
    //{
    //    public static readonly ZoomLevelToTextFormattingModeConverter Instance = new ZoomLevelToTextFormattingModeConverter();
		
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (((double)value) == 1.0)
    //            return TextFormattingMode.Display;
    //        else
    //            return TextFormattingMode.Ideal;
    //    }
		
    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }
    //}
}
