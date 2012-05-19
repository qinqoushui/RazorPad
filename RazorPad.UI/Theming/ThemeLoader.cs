using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace RazorPad.UI.Theming
{
    [Export]
    public class ThemeLoader
    {
        public IEnumerable<Theme> LoadThemes(string selectedTheme = null)
        {
            var themeDirectory = Path.Combine(Environment.CurrentDirectory, "themes");

            if (!Directory.Exists(themeDirectory))
                return Enumerable.Empty<Theme>();

            var themes =
                from file in Directory.GetFiles(themeDirectory, "*.xaml")
                let name = Path.GetFileNameWithoutExtension(file)
                let selected = string.Equals(name, selectedTheme, StringComparison.OrdinalIgnoreCase)
                select new Theme
                           {
                               FilePath = file,
                               Name = name,
                               Selected = selected,
                           };
            
            return themes;
        }
    }
}