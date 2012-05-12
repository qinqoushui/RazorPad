// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace RazorPad.UI.Editors.Folding
{
    public interface IHtmlReaderFactory
    {
        HtmlReader CreateHtmlReader(string html);
    }
}