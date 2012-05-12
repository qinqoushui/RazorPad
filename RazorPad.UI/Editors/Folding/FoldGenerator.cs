// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using NLog;

namespace RazorPad.UI.Editors.Folding
{
    public class FoldGenerator : IFoldGenerator
    {
        ITextEditorWithParseInformationFolding textEditor;
        IFoldParser foldParser;
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public FoldGenerator(ITextEditorWithParseInformationFolding textEditor,
                                  IFoldParser foldParser)
        {
            this.textEditor = textEditor;
            this.foldParser = foldParser;
            IsParseInformationFoldingEnabled = false;
            this.textEditor.InstallFoldingManager();
        }

        bool IsParseInformationFoldingEnabled
        {
            get { return textEditor.IsParseInformationFoldingEnabled; }
            set { textEditor.IsParseInformationFoldingEnabled = value; }
        }

        public void Dispose()
        {
            IsParseInformationFoldingEnabled = true;
            textEditor.Dispose();
        }

        public void GenerateFolds()
        {
            try
            {
                textEditor.UpdateFolds(GetFolds());
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
            }
        }

        IEnumerable<NewFolding> GetFolds()
        {
            return foldParser.GetFolds(textEditor.GetTextSnapshot());
        }
    }
}
