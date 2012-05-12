// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace RazorPad.UI.Editors.Folding
{
	public class TextEditorWithParseInformationFoldingFactory : ITextEditorWithParseInformationFoldingFactory
	{
		public ITextEditorWithParseInformationFolding CreateTextEditor(ITextEditor textEditor)
		{
			return new TextEditorWithParseInformationFolding(textEditor);
		}
	}
}
