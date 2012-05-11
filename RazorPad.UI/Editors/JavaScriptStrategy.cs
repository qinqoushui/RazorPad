using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

namespace RazorPad.UI.Editors
{
    internal class JavaScriptStrategy : ICodeEditorStrategy
    {
        public void Apply(CodeEditor editor)
        {
            editor.Editor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
            editor.InitializeFolding(new BraceFoldingStrategy());
            editor.Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
        }
    }
}