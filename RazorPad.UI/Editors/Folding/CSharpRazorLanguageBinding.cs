// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using RazorPad.UI.Editors.Services;

namespace RazorPad.UI.Editors.Folding
{
	public class CSharpRazorLanguageBinding : ILanguageBinding
	{
		ITextEditorWithParseInformationFoldingFactory textEditorFactory;
		IFoldGeneratorFactory foldGeneratorFactory;
		IFoldGenerator foldGenerator;

        public CSharpRazorLanguageBinding(
			ITextEditorWithParseInformationFoldingFactory textEditorFactory,
			IFoldGeneratorFactory foldGeneratorFactory)
		{
			this.textEditorFactory = textEditorFactory;
			this.foldGeneratorFactory = foldGeneratorFactory;
		}
		
		public IFormattingStrategy FormattingStrategy {
			get { return new DefaultFormattingStrategy(); }
		}
		
		public LanguageProperties Properties {
			get { return LanguageProperties.None; }
		}

        /// <summary>
        /// Provides access to the bracket search logic for this language.
        /// </summary>
        public IBracketSearcher BracketSearcher
        {
            get
            {
                return null;
            }
        }

        public void Attach(ITextEditor editor)
        {
            Attach(textEditorFactory.CreateTextEditor(editor));
        }

        void Attach(ITextEditorWithParseInformationFolding editor)
        {
            foldGenerator = foldGeneratorFactory.CreateFoldGenerator(editor);
        }
		
		
		public void Detach()
		{
			foldGenerator.Dispose();
		}
	}
}
