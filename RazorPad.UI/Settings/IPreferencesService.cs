namespace RazorPad.UI.Settings
{
    public interface IPreferencesService
    {
        Preferences Load();
        void Save(Preferences preferences);
    }
}