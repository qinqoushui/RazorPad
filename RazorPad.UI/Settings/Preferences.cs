using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace RazorPad.UI.Settings
{
    public class Preferences
    {
        [Export]
        public static Preferences Current
        {
            get { return _current ?? Default; }
            set { _current = value; }
        }
        private volatile static Preferences _current;

        public static Preferences Default
        {
            get
            {
                return new Preferences
                           {
                               AutoExecute = true,
                               AutoSave = true,
                               FontSize = 12,
                               GlobalNamespaceImports = new[] { "RazorPad", "RazorPad.Compilation" },
                               ModelProvider = "Json",
                               ShowDemoTemplate = true,
                           };
            }
        }

        public bool? AutoExecute { get; set; }
        public bool? AutoSave { get; set; }
        public string ModelProvider { get; set; }
        public IEnumerable<string> LoadedFiles { get; set; }
        public double? FontSize { get; set; }
        public IEnumerable<string> GlobalNamespaceImports { get; set; }
        public IEnumerable<string> RecentFiles { get; set; }
        public IEnumerable<string> RecentReferences { get; set; }
        public bool? ShowDemoTemplate { get; set; }
        public string Theme { get; set; }
    }
}
