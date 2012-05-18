using System.IO;
using System.Text;

namespace RazorPad.Persistence
{
    public interface IRazorDocumentSource
    {
        Encoding Encoding { get; set; }

        bool CanLoad(string uri);
        bool CanLoad(Stream stream);
        bool CanSave(RazorDocument document, string uri);
        bool CanSave(RazorDocument document, Stream stream);

        RazorDocument Load(string uri);
        RazorDocument Load(Stream stream);

        void Save(RazorDocument document, string uri);
        void Save(RazorDocument document, Stream stream);
    }
}