using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorPad.UI.Editors.Folding
{
    public class RazorFoldableSpans
    {
        IEnumerable<Span> htmlSpans;
        RazorCodeLanguage codeLanguage;
        private ParserResults parserResults;

        public IEnumerable<SyntaxTreeNode> CodeSpans { get; private set; }


        public RazorFoldableSpans(string markup, string fileExtension)
        {
            codeLanguage = RazorCodeLanguage.GetLanguageByExtension(fileExtension);
            ReadHtmlSpans(markup);
            ReadCodeSpans(markup);
        }

        private void ReadCodeSpans(string markup)
        {
            if (!IsTemplateParsed()) ParseTemplate(markup);
            CodeSpans = parserResults.Document.Children.Where(span => span.IsBlock);
        }

        void ReadHtmlSpans(string html)
        {
            if(!IsTemplateParsed()) ParseTemplate(html);
            htmlSpans = parserResults.Document.Flatten().Where(span => span.Kind == SpanKind.Markup);
        }

        void ParseTemplate(string html)
        {
            var parser = new RazorParser(codeLanguage.CreateCodeParser(), new HtmlMarkupParser());
            parserResults = parser.Parse(new StringReader(html));
        }

        public bool IsHtml(int offset)
        {
            return offset < 0 || HtmlSpansContainOffset(offset);
        }

        bool HtmlSpansContainOffset(int offset)
        {
            return htmlSpans.Any(span => IsInSpan(span, offset));
        }

        bool IsInSpan(Span span, int offset)
        {
            var spanOffset = span.Start.AbsoluteIndex;
            return (offset >= spanOffset) && (offset < spanOffset + span.Length);
        }

        bool IsTemplateParsed()
        {
            //return markupSpans != null;
            return parserResults != null;
        }
    }
}