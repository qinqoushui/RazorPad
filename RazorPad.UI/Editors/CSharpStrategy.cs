using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

namespace RazorPad.UI.Editors
{
    internal class CSharpStrategy : ICodeEditorStrategy
    {
        public void Apply(CodeEditor editor)
        {
            editor.Editor.TextArea.IndentationStrategy = new CSharpIndentationStrategy(editor.Editor.Options);
            editor.InitializeFolding(new BraceFoldingStrategy());
            editor.Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        }
    }
}