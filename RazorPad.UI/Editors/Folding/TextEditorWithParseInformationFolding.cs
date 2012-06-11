// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Editors.Folding
{
	public class TextEditorWithParseInformationFolding : ITextEditorWithParseInformationFolding
	{
		public ITextEditor TextEditor { get; set; }
	
		FoldingManager foldingManager;
		
		public TextEditorWithParseInformationFolding(ITextEditor textEditor)
		{
			this.TextEditor = textEditor;
		    IsParseInformationFoldingEnabled = true;
		}
		
		public void InstallFoldingManager()
		{
			var textEditorAdapter = TextEditor as AvalonEditTextEditorAdapter;
			if (textEditorAdapter != null) {
				foldingManager = FoldingManager.Install(textEditorAdapter.TextEditor.TextArea);
			}
		}

        public bool IsParseInformationFoldingEnabled { get; set; }
		
		
		
		public void UpdateFolds(IEnumerable<NewFolding> folds)
		{
			foldingManager.UpdateFoldings(folds, -1);
		}
		
		public string GetTextSnapshot()
		{
			return TextEditor.Document.CreateSnapshot().Text;
		}
		
		public void Dispose()
		{
			FoldingManager.Uninstall(foldingManager);
		}
	}
}
