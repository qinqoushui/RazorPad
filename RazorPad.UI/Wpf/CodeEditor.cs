using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Wpf
{
	public class CodeEditor : TextEditor
	{

		//public string Code
		//{
		//	get { return Document.Text; }
		//	set
		//	{
		//		if (Document.Text == value)
		//			return;

		//		Document.Text = value;
		//		TriggerPropertyChanged("Code");
		//	}
		//}

		public static readonly int DefaultTextChangedEventDelay = (int)TimeSpan.FromSeconds(.5).TotalMilliseconds;

		private readonly Timer _textChangedTimer;

		public new string Text
		{
			get
			{
				return GetValue(TextProperty) as string;
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}



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
			document.Text = e.NewValue as string ?? string.Empty;
			editor.CaretOffset = 0;
			document.UndoStack.ClearAll();
			
		}

		public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code",
			typeof(String), typeof(CodeEditor), new FrameworkPropertyMetadata(string.Empty, OnCodeChanged)
			{
				BindsTwoWayByDefault = true
			});



		public CodeEditor()
		{
			ShowLineNumbers = true;
			//Document = new TextDocument();
			
			//_textChangedTimer = new Timer { Interval = DefaultTextChangedEventDelay };
			//InitializeTextChangedTimer();
			TextChanged += (sender, e) =>
			               	{
			               		Text = Document.Text;
			               	};

			
		}

		//private void InitializeTextChangedTimer()
		//{
		//	_textChangedTimer.Elapsed += (x, y) =>
		//	{
		//		var bindingExpression = GetBindingExpression(TextProperty);

		//		if (bindingExpression != null)
		//		{
		//			Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
		//				new Action(bindingExpression.UpdateSource));
		//		}
		//		_textChangedTimer.Stop();
		//	};
		//}

		protected void InitializeFolding(AbstractFoldingStrategy foldingStrategy)
		{
			var foldingManager = FoldingManager.Install(TextArea);
			//foldingStrategy.UpdateFoldings(foldingManager, Document);

			var foldingUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
			foldingUpdateTimer.Tick += (o, args) => foldingStrategy.UpdateFoldings(foldingManager, Document);

			foldingUpdateTimer.Start();
		}

		private static void OnTextChangedInternal(object sender, DependencyPropertyChangedEventArgs e)
		{
			var editor = sender as CodeEditor;
			if (editor == null) return;
			var document = editor.Document;
			document.Text = e.NewValue as string ?? string.Empty;


		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
			typeof(String), typeof(CodeEditor), new FrameworkPropertyMetadata(string.Empty, OnTextChangedInternal)
			{
				BindsTwoWayByDefault = true
			});

		
	}
}
