using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RazorPad.Persistence
{
    public class RazorTemplateFileSource : IRazorDocumentSource
    {
        public static readonly IList<string> Extensions = 
            new List<string> { ".cshtml", ".vbhtml", ".txt" };

        public Encoding Encoding { get; set; }

        public RazorTemplateFileSource()
        {
            Encoding = Encoding.UTF8;
        }

        public bool CanLoad(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return false;

            return Extensions.Any(x => uri.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        public bool CanLoad(Stream stream)
        {
            return stream != null && stream.CanRead;
        }

        public bool CanSave(RazorDocument document, string uri)
        {
            return document.DocumentKind == RazorDocumentKind.TemplateOnly
                || CanLoad(uri);
        }

        public bool CanSave(RazorDocument document, Stream stream)
        {
            if(document == null || stream == null || !stream.CanRead)
                return false;

            return CanSave(document, document.Filename);
        }

        public RazorDocument Load(string uri)
        {
            using (var stream = File.OpenRead(uri))
            {
                var document = Load(stream);
                document.Filename = uri;
                return document;
            }
        }

        public RazorDocument Load(Stream stream)
        {
            var template = new StreamReader(stream, Encoding).ReadToEnd();
            var document = new RazorDocument(template);
            return document;
        }

        public void Save(RazorDocument document, string uri)
        {
            var destination = uri ?? document.Filename;

            if (string.IsNullOrWhiteSpace(destination))
                throw new ApplicationException("No filename specified!");

            document.DocumentKind = RazorDocument.GetDocumentKind(uri);

            using (var stream = File.Open(destination, FileMode.Create, FileAccess.Write))
            {
                Save(document, stream);

                try { stream.Flush(); }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error flushing file stream: " + ex);
                }
            }
        }

        public void Save(RazorDocument document, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
                writer.Write(document.Template);
        }
    }
}