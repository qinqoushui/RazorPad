// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorPad.UI.Editors.Folding
{
    public class RazorSpans
    {
        List<Span> spans;
        RazorCodeLanguage codeLanguage;

        public RazorSpans(string markup, string fileExtension)
        {
            codeLanguage = RazorCodeLanguage.GetLanguageByExtension(fileExtension);
            ReadBlockSpans(markup);
        }

        public string CodeLanguageName
        {
            get { return codeLanguage.LanguageName; }
        }

        void ReadBlockSpans(string markup)
        {
            var razorEngineHost = new RazorEngineHost(codeLanguage);
            var engine = new RazorTemplateEngine(razorEngineHost);
            var results = engine.ParseTemplate(new StringReader(markup));
            spans = new List<Span>(results.Document.Flatten());
            spans.RemoveAll(span => !span.IsBlock);
        }

        public bool IsHtml(int offset)
        {
            return offset < 0 || HtmlSpansContainOffset(offset);
        }

        bool HtmlSpansContainOffset(int offset)
        {
            return spans.Any(span => IsInSpan(span, offset));
        }

        bool IsInSpan(Span span, int offset)
        {
            var spanOffset = span.Start.AbsoluteIndex;
            return (offset >= spanOffset) && (offset < spanOffset + span.Length);
        }
    }
}