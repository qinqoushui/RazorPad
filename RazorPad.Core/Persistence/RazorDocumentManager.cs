using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RazorPad.Persistence
{
    [Export]
    public class RazorDocumentManager : IRazorDocumentSource
    {
        private readonly IEnumerable<IRazorDocumentSource> _documentSources;

        public Encoding Encoding { get; set; }

        [ImportingConstructor]
        public RazorDocumentManager([ImportMany]IEnumerable<IRazorDocumentSource> documentSources)
        {
            _documentSources = (documentSources ?? Enumerable.Empty<IRazorDocumentSource>());

            // Always add the plain Razor Template file source at the end of the list
            _documentSources = _documentSources.Union(new [] { new RazorTemplateFileSource() });

            Encoding = Encoding.UTF8;
        }

        public bool CanLoad(string uri)
        {
            return _documentSources.Any(x => x.CanLoad(uri));
        }

        public bool CanLoad(Stream stream)
        {
            return _documentSources.Any(x => x.CanLoad(stream));
        }

        public bool CanSave(RazorDocument document, string uri)
        {
            throw new NotImplementedException();
        }

        public bool CanSave(RazorDocument document, Stream stream)
        {
            return _documentSources.Any(x => x.CanSave(document, stream));
        }


        public RazorDocument Parse(string content)
        {
            using (var stream = new MemoryStream(Encoding.GetBytes(content)))
                return Load(stream);
        }


        public RazorDocument Load(string uri)
        {
            var source = _documentSources.FirstOrDefault(x => x.CanLoad(uri));

            if (source == null)
                throw new RazorDocumentSourceNotFoundException();

            return source.Load(uri);
        }

        public RazorDocument Load(Stream stream)
        {
            var source = _documentSources.FirstOrDefault(x => x.CanLoad(stream));

            if (source == null)
                throw new RazorDocumentSourceNotFoundException();

            return source.Load(stream);
        }

        public void Save(RazorDocument document, string uri)
        {
            var source = _documentSources.FirstOrDefault(x => x.CanSave(document, uri));
            source = source ?? _documentSources.LastOrDefault();

            if (source == null)
                throw new RazorDocumentSourceNotFoundException();

            source.Save(document, uri);
        }

        public void Save(RazorDocument document, Stream stream)
        {
            var source = _documentSources.FirstOrDefault(x => x.CanSave(document, stream));
            source = source ?? _documentSources.LastOrDefault();

            if (source == null)
                throw new RazorDocumentSourceNotFoundException();

            source.Save(document, stream);
        }
    }

    public class RazorDocumentSourceNotFoundException : Exception
    {
    }
}