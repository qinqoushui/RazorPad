using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation;

namespace RazorPad.UI.Editors
{
    internal class RazorStrategy : ICodeEditorStrategy
    {
        public void Apply(CodeEditor editor)
        {
            editor.Editor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
            editor.InitializeFolding(new XmlFoldingStrategy());
            editor.Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("HTML");
        }
    }
}