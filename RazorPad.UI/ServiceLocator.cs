using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace RazorPad
{
    public static class ServiceLocator
    {
        static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static CompositionContainer Container { get; private set; }

        public static void Initialize(params string[] assemblyNames)
        {
            var assemblies = assemblyNames.Select(Assembly.Load);
            Initialize(assemblies.ToArray());
        }

        public static void Initialize(params Assembly[] assemblies)
        {
            if(Container != null)
                return;

            Log.Info("Initializing Service Locator...");

            var assemblyCatalogs = new AggregateCatalog(
                    assemblies.Select(x => new AssemblyCatalog(x))
                );

            var plugins = GetPluginsCatalog();

            Container = new CompositionContainer(new AggregateCatalog(assemblyCatalogs, plugins));

            Log.Info("Service Locator initialized");
        }

        public static TService Get<TService>(string name = null)
        {
            if (name != null)
                return Container.GetExportedValue<TService>(name);

            return Container.GetExportedValue<TService>();
        }

        public static IEnumerable<TService> GetMany<TService>(string name = null)
        {
            if (name != null)
                return Container.GetExportedValues<TService>(name);

            return Container.GetExportedValues<TService>();
        }

        public static void Inject(object target)
        {
            Container.ComposeParts(target);
        }

        private static DirectoryCatalog GetPluginsCatalog()
        {
            string pluginPath = Path.Combine(Environment.CurrentDirectory, "plugins");

            Log.Info("Looking for plugins in " + pluginPath);

            if (!Directory.Exists(pluginPath))
            {
                Log.Debug("Plugin directory {0} doesn't exist - creating...", pluginPath);
                Directory.CreateDirectory(pluginPath);
            }

            var plugins = new DirectoryCatalog(pluginPath);

            Log.Info(string.Format("Found {0} parts in {1} plugin assemblies", 
                     plugins.Parts.Count(), plugins.LoadedFiles.Count));

            return plugins;
        }
    }
}
