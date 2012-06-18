using System.Collections.Generic;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorPad.UI.Editors.Folding
{
    public class RazorHtmlReader : HtmlReader
    {
        RazorMarkupCharacterReader reader;

        public RazorHtmlReader(string html, string fileExtension)
            : this(new RazorMarkupCharacterReader(html, fileExtension))
        {
        }

        public RazorHtmlReader(RazorMarkupCharacterReader reader)
            : base(reader)
        {
            this.reader = reader;
        }

        protected override bool IsHtml()
        {
            return reader.IsHtml;
        }

        public IEnumerable<SyntaxTreeNode> CodeSpans
        {
            get { return reader.CodeSpans; }
        }
    }
}