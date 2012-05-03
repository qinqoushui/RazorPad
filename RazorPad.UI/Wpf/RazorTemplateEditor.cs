using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace RazorPad.UI.Wpf
{
	public class RazorTemplateEditor : CodeEditor
	{
		public RazorTemplateEditor()
		{
			TextArea.IndentationStrategy = new RazorIndentationStrategy(Options);
			InitializeFolding(new XmlFoldingStrategy());
            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("HTML");
		}

	}
}
