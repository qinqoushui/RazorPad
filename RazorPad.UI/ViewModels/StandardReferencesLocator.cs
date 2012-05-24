using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using NLog;

namespace RazorPad.ViewModels
{
	public class StandardReferencesLocator
	{
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        private static readonly IEnumerable<string> KnownRegistryLocations = new[] {
                @"SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.5.50131\AssemblyFoldersEx\Visual Studio v11.0 Reference Assemblies",
                @"SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Visual Studio v11.0 Reference Assemblies",
                @"SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Public Assemblies v11.0",
                @"SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\ASP.NET MVC 4",
                @"SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\ASP.NET MVC 3",
            };

		private static readonly IEnumerable<string> KnownFolderLocations = new[] {
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5",
				@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5",
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0",
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v3.5",
				@"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5",
                @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5",
                @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0",
                @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v3.5",
            };

		private static readonly Lazy<IEnumerable<string>> StandardDotNetReferencePaths = 
            new Lazy<IEnumerable<string>>(DiscoverStandardDotNetReferencePaths);


        public IEnumerable<AssemblyReference> GetStandardReferences()
	    {
            var paths = (StandardDotNetReferencePaths.Value ?? Enumerable.Empty<string>()).ToArray();

            Log.Debug("Standard .NET References: " + string.Join(", ", paths));

            foreach (var path in paths)
            {
                AssemblyReference assemblyReference;
                string message;

                Log.Debug("Loading standard reference {0}... ", path);

                var isLoadable = AssemblyReference.TryLoadReference(path, out assemblyReference, out message);

                if (!isLoadable)
                {
                    Log.Warn("Reference {0} NOT loaded.", path);
                    continue;
                }

                Log.Info("Standard reference {0} loaded.", path);

                assemblyReference.IsStandard = true;

                yield return assemblyReference;
            }
	    }


	    private static IEnumerable<string> DiscoverStandardDotNetReferencePaths()
	    {
	        var registryFolders = KnownRegistryLocations.SelectMany(GetRegistryValues);
	        var discoveredLocations = registryFolders.Union(KnownFolderLocations).Where(Directory.Exists);
	        var paths = discoveredLocations.SelectMany(l => Directory.GetFiles(l, "*.dll"));
	        
            return paths.ToArray();
	    }

	    private static IEnumerable<string> GetRegistryValues(string registryKey)
		{
			var key = Registry.LocalMachine.OpenSubKey(registryKey);

			if (key != null)
			{
				yield return key.GetValue("").ToString();

				if (key.SubKeyCount > 0)
				{
					var folderNames =
						key.GetSubKeyNames()
                            .Select(key.OpenSubKey)
                            .Where(subKey => subKey != null)
							.Select(subKey => subKey.GetValue("") as string)
                            .Where(path => !string.IsNullOrEmpty(path));

				    foreach (var folderName in folderNames)
				    {
                        yield return folderName;
                    }
				}
            }
		}
	}
}
