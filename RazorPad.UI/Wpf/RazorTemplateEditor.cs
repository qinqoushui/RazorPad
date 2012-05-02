using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using RazorPad.UI.AvalonEdit;

namespace RazorPad.UI.Wpf
{
	public class RazorTemplateEditor : CodeEditor
	{
		public RazorTemplateEditor()
		{
			TextArea.IndentationStrategy = new RazorIndentationStrategy(Options);
			InitializeFolding(new XmlFoldingStrategy());
		}

	}
}
