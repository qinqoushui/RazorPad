using System;
using System.Linq;
using System.Windows.Input;

namespace RazorPad.UI.Editors.Folding
{
    public class ReactiveFoldGenerator : IFoldGenerator
    {
    	readonly IFoldGenerator _foldGenerator;


		public ReactiveFoldGenerator(
			ITextEditor textEditor, 
            IFoldGenerator foldGenerator)
        {
            _foldGenerator = foldGenerator;
			var textEditorAdapter = textEditor as AvalonEditTextEditorAdapter;

            GenerateFolds();

			if (textEditorAdapter != null) textEditorAdapter.KeyPress += (sender, args) =>
			{
				if (args.Key == Key.Enter)
					GenerateFolds();
			};
				
        }

        public void GenerateFolds()
        {
            _foldGenerator.GenerateFolds();
        }

        public void Dispose()
        {
            _foldGenerator.Dispose();
        }
    }
}
