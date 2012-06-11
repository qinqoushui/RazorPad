// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Editors.Folding
{
    public interface ITextEditorWithParseInformationFolding : IDisposable
    {
        bool IsParseInformationFoldingEnabled { get; set; }

        void UpdateFolds(IEnumerable<NewFolding> folds);
        void InstallFoldingManager();
        string GetTextSnapshot();

		ITextEditor TextEditor { get; set; }
    }
}
