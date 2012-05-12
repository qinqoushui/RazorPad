// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace RazorPad.UI.Editors.Folding
{
    public class RazorHtmlReaderFactory : IHtmlReaderFactory
    {
        public RazorHtmlReaderFactory(string fileExtension)
        {
            this.FileExtension = fileExtension;
        }

        string FileExtension { get; set; }

        public HtmlReader CreateHtmlReader(string html)
        {
            return new RazorHtmlReader(html, FileExtension);
        }
    }
}