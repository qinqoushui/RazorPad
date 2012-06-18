// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorPad.UI.Editors.Folding
{
    public class RazorMarkupCharacterReader : CharacterReader
    {
        RazorFoldableSpans foldableSpans;

        public RazorMarkupCharacterReader(string html, string fileExtension)
            : base(html)
        {
            foldableSpans = new RazorFoldableSpans(html, fileExtension);
        }

        public bool IsHtml
        {
            get { return foldableSpans.IsHtml(CurrentCharacterOffset); }
        }

        public IEnumerable<SyntaxTreeNode> CodeSpans
        {
            get { return foldableSpans.CodeSpans; }
        }
    }
}