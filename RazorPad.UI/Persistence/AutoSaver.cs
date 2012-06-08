using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using RazorPad.Persistence;
using RazorPad.ViewModels;

namespace RazorPad.UI.Persistence
{
    [Export]
    public class AutoSaver
    {
        private readonly IRazorDocumentSource _source;

        public string AutoSavePath { get; set; }

        [ImportingConstructor]
        public AutoSaver(IRazorDocumentSource source)
        {
            Contract.Requires(source != null);
            _source = source;
            AutoSavePath = Path.Combine(Directory.GetCurrentDirectory(), "AutoSave.razorpad");
        }

        /// <summary>
        /// Removes any existing auto-save documents
        /// </summary>
        public void Clear()
        {
            try
            {
                if (File.Exists(AutoSavePath))
                    File.Delete(AutoSavePath);
            }
            catch
            {
                // We don't really care if it gets cleaned up or not, 
                // so just log the warning and continue
                Trace.TraceWarning("Couldn't clean up Auto-Save file");
            }
        }

        /// <summary>
        /// Loads the last auto-saved document
        /// </summary>
        /// <returns>The last auto-saved document; null if there is none</returns>
        public RazorDocument Load()
        {
            RazorDocument document = null;

            if (_source.CanLoad(AutoSavePath))
            {
                document = _source.Load(AutoSavePath);
                Clear();
            }

            return document;
        }

        /// <summary>
        /// Auto-Saves the document
        /// </summary>
        /// <param name="document">The document to save</param>
        public void Save(RazorDocument document)
        {
            if(_source.CanSave(document, AutoSavePath))
                _source.Save(document, AutoSavePath);
        }
    }
}
