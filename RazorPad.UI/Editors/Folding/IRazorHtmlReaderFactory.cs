namespace RazorPad.UI.Editors.Folding
{
    public interface IRazorHtmlReaderFactory
    {
        RazorHtmlReader CreateHtmlReader(string html);
    }
}