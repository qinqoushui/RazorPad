using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using NLog;
using RazorPad.UI.Settings;

namespace RazorPad.ViewModels
{
    public class ReferencesViewModel
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public SearchableReferencesViewModel StandardReferences { get; set; }
        public SearchableReferencesViewModel RecentReferences { get; set; }
        public SearchableReferencesViewModel InstalledReferences { get; set; }

        public ReferencesViewModel(IEnumerable<AssemblyReference> loadedReferences)
        {
            Log.Info(() => string.Format("Loaded references: {0}", string.Join(", ", loadedReferences)));

            var standardReferences = new StandardReferencesLocator().GetStandardReferences().ToArray();
            var recentReferences = (Preferences.Current.RecentReferences ?? Enumerable.Empty<string>()).Where(File.Exists).Select(x => new AssemblyReference(x) { IsRecent = true }).ToArray();
            var allReferences = loadedReferences.Union(standardReferences).Union(recentReferences).ToArray();


            StandardReferences = new SearchableReferencesViewModel(standardReferences);
            StandardReferences.References.ItemPropertyChanged += StandardReferences_ListChanged;

            RecentReferences = new SearchableReferencesViewModel(recentReferences);
            RecentReferences.References.ItemPropertyChanged += RecentReferences_ListChanged;

            InstalledReferences = new SearchableReferencesViewModel(allReferences.Where(r => r.IsInstalled));
        }



        void StandardReferences_ListChanged(object sender, PropertyChangedEventArgs e)
        {
            // prevent stack overflow
            StandardReferences.References.ItemPropertyChanged -= StandardReferences_ListChanged;
            RecentReferences.References.ItemPropertyChanged -= RecentReferences_ListChanged;

            var reference = sender as AssemblyReference;
            if (reference == null) return;

            if (reference.IsInstalled)
            {
                if (!InstalledReferences.References.Contains(reference))
                {
                    InstalledReferences.References.Add(reference);
                }

                // find the handle by equality operator
                var recentReferenceIndex = RecentReferences.References.IndexOf(reference);

                // if not found, add it
                if (recentReferenceIndex == -1)
                {
                    RecentReferences.References.Add(reference);
                }
                // reassign the reference for auto syncing
                else
                {
                    RecentReferences.References.RemoveAt(recentReferenceIndex);
                    RecentReferences.References.Add(reference);
                }

            }
            else
            {
                var index = InstalledReferences.References.IndexOf(reference);
                if (index >= 0) InstalledReferences.References.RemoveAt(index);

            }

            StandardReferences.References.ItemPropertyChanged += StandardReferences_ListChanged;
            RecentReferences.References.ItemPropertyChanged += RecentReferences_ListChanged;
        }

        void RecentReferences_ListChanged(object sender, PropertyChangedEventArgs e)
        {
            // prevent stack overflow
            RecentReferences.References.ItemPropertyChanged -= RecentReferences_ListChanged;

            var reference = sender as AssemblyReference;
            if (reference == null) return;

            if (reference.IsInstalled)
            {
                if (!InstalledReferences.References.Contains(reference))
                {
                    InstalledReferences.References.Add(reference);
                }
            }
            else
            {
                var index = InstalledReferences.References.IndexOf(reference);
                if (index >= 0) InstalledReferences.References.RemoveAt(index);
            }

            Preferences.Current.RecentReferences =
                RecentReferences.References
                    .Distinct()
                    .Take(50)
                    .Select(r => r.Location);


            RecentReferences.References.ItemPropertyChanged += RecentReferences_ListChanged;
        }

        public bool TryAddReference(string filePath, out string message)
        {
            AssemblyReference assemblyReference;
            AssemblyReference.TryLoadReference(filePath, out assemblyReference, out message);

            if (assemblyReference == null)
                return false;

            assemblyReference.IsInstalled = assemblyReference.IsRecent = true;
            RecentReferences.References.Add(assemblyReference);
            InstalledReferences.References.Add(assemblyReference);
                
            return true;
        }
    }
}
