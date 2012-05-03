using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using RazorPad.UI.AvalonEdit;

namespace RazorPad.UI.Wpf
{
    public class JsonCodeEditor : CodeEditor
    {
        public JsonCodeEditor()
        {
            TextArea.IndentationStrategy = new JsonIndentationStrategy(Options);
            InitializeFolding(new BraceFoldingStrategy());
            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
        }
    }
}