using System;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using RazorPad.UI.AvalonEdit;
using RazorPad.ViewModels;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace RazorPad.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RazorTemplateEditor : UserControl
    {
        private static readonly int TemplateTextChangedEventDelay = (int)TimeSpan.FromSeconds(.5).TotalMilliseconds;

        Timer _templateTextChangedTimer;

        protected RazorTemplateEditorViewModel ViewModel
        {
            get { return (RazorTemplateEditorViewModel) DataContext; }
        }

        public RazorTemplateEditor()
        {
            InitializeComponent();
            InitializeTemplateTextChangedTimer();

            //InitializeAvalonEditor();

            
        }


		//protected void InitializeAvalonEditor()
		//{
		//	textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy(textEditor.Options);
		//	textEditor.ShowLineNumbers = true;

		//	InitializeFolding();
		//}


        #region Folding


		//FoldingManager _foldingManager;
		//AbstractFoldingStrategy _foldingStrategy;

		//protected void InitializeFolding()
		//{
		//	_foldingStrategy = new BraceFoldingStrategy();
		//	_foldingManager = FoldingManager.Install(textEditor.TextArea);
		//	_foldingStrategy.UpdateFoldings(_foldingManager, textEditor.Document);

		//	var foldingUpdateTimer = new DispatcherTimer();
		//	foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
		//	foldingUpdateTimer.Tick += FoldingUpdateTimerTick;
		//	foldingUpdateTimer.Start();
		//}

		//void FoldingUpdateTimerTick(object sender, EventArgs e)
		//{
		//	if (_foldingStrategy != null)
		//	{
		//		_foldingStrategy.UpdateFoldings(_foldingManager, textEditor.Document);
		//	}
		//}

        #endregion

        private void InitializeTemplateTextChangedTimer()
        {
            _templateTextChangedTimer = new Timer { Interval = TemplateTextChangedEventDelay };

            _templateTextChangedTimer.Tick += (x, y) =>
            {
                //textEditor
                //    .GetBindingExpression(textEditor.Text.TextProperty)
                //    .UpdateSource();

                _templateTextChangedTimer.Stop();
            };
        }

        private void TemplateTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_templateTextChangedTimer.Enabled)
                _templateTextChangedTimer.Stop();
            
            _templateTextChangedTimer.Start();
        }
    }
}
