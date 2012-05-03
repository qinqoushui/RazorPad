using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Indentation;

namespace RazorPad.UI.Wpf
{
    public class JsonIndentationStrategy : DefaultIndentationStrategy
    {
        public JsonIndentationStrategy(TextEditorOptions options)
        {

        }

        public JsonIndentationStrategy()
            : this(new TextEditorOptions())
        {

        }
    }
}