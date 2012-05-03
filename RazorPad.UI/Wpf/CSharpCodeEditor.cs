using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using RazorPad.UI.AvalonEdit;

namespace RazorPad.UI.Wpf
{
    public class CSharpCodeEditor : CodeEditor
    {
        public CSharpCodeEditor()
        {
            TextArea.IndentationStrategy = new CSharpIndentationStrategy(Options);
            InitializeFolding(new BraceFoldingStrategy());
            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        }
    }
}