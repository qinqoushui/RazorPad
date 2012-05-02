using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Indentation;

namespace RazorPad.UI.Wpf
{
	public class RazorIndentationStrategy : DefaultIndentationStrategy
	{
		public RazorIndentationStrategy()
			: this(new TextEditorOptions())
		{

		}

		public RazorIndentationStrategy(TextEditorOptions options)
		{

		}
	}
}
