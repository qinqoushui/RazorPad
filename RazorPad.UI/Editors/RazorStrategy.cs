using System.IO;
using System.Reflection;
using System.Xml;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Indentation;
using System.Linq;
using RazorPad.UI.Editors.CodeCompletion;
using RazorPad.UI.Editors.Folding;

namespace RazorPad.UI.Editors
{
    internal class RazorStrategy : ICodeEditorStrategy
    {
        public void Apply(CodeEditor editor)
        {
            editor.Editor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
            editor.Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("CSharpRazor") ?? LoadCSharpRazorDefinition();

            var cSharpRazorLanguageBinding = new CSharpRazorLanguageBinding(new TextEditorWithParseInformationFoldingFactory(),
                                                    new RazorFoldGeneratorFactory(".cshtml"));

            cSharpRazorLanguageBinding.Attach(new CodeCompletionEditorAdapter(editor.Editor));

        }

        private IHighlightingDefinition LoadCSharpRazorDefinition()
        {
            using (var s = GetType().Assembly.GetManifestResourceStream("RazorPad.UI.Resources.CSharpRazor.xshd"))
            using (var reader = new XmlTextReader(s))
                return HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }
    }
}