using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace RazorPad.UI.Settings
{
    [Export(typeof(IPreferencesService))]
    public class PreferencesService : IPreferencesService
    {
        private const string Filename = @".\preferences";

        public Encoding Encoding { get; set; }

        public PreferencesService()
        {
            Encoding = Encoding.UTF8;
        }

        public Preferences Load()
        {
            if (File.Exists(Filename) == false)
                return Preferences.Default;

            var serializedPreferences = File.ReadAllText(Filename);
            var serializer = new JavaScriptSerializer();
            var preferences = serializer.Deserialize<Preferences>(serializedPreferences);
            return preferences;
        }

        public void Save(Preferences preferences)
        {
            if (preferences == null)
                return;

            var serializer = new JavaScriptSerializer();
            var serialized = serializer.Serialize(preferences);

            File.WriteAllText(Filename, serialized, Encoding);
        }
    }
}