
namespace RazorPad.UI.Editors.Folding
{
    public class RazorHtmlReaderFactory : IRazorHtmlReaderFactory
    {
        public RazorHtmlReaderFactory(string fileExtension)
        {
            this.FileExtension = fileExtension;
        }

        string FileExtension { get; set; }

        public RazorHtmlReader CreateHtmlReader(string html)
        {
            return new RazorHtmlReader(html, FileExtension);
        }
    }
}