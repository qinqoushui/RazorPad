using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Wpf
{
	public class CodeEditor : TextEditor
	{
		public string Code
		{
			get { return GetValue(CodeProperty) as string; }
			set { SetValue(CodeProperty, value); }
		}

		public static void OnCodeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var editor = sender as CodeEditor;
			if (editor == null) return;
			var document = editor.Document;
			document.Text = e.NewValue as string;
		}

		public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code",
			typeof(String), typeof(CodeEditor), new FrameworkPropertyMetadata(string.Empty, OnCodeChanged)
			{
				BindsTwoWayByDefault = true
			});

		public CodeEditor()
		{
			ShowLineNumbers = true;
			Document = new TextDocument(Code);
		}

		FoldingManager _foldingManager;
		AbstractFoldingStrategy _foldingStrategy;

		protected void InitializeFolding(AbstractFoldingStrategy foldingStrategy)
		{
			_foldingStrategy = foldingStrategy;
			_foldingManager = FoldingManager.Install(TextArea);
			_foldingStrategy.UpdateFoldings(_foldingManager, Document);

			var foldingUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
			foldingUpdateTimer.Tick += (o, args) =>
			{
				if (_foldingStrategy != null)
				{
					_foldingStrategy.UpdateFoldings(_foldingManager, Document);
				}
			};

			foldingUpdateTimer.Start();
		}
	}
}
