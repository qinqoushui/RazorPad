using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AvalonDock;
using NLog;
using NLog.Config;
using RazorPad.Compilation.Hosts;
using RazorPad.Providers;
using RazorPad.UI;
using RazorPad.UI.Settings;
using RazorPad.UI.Theming;
using RazorPad.UI.Util;
using RazorPad.ViewModels;

namespace RazorPad.Views
{
    public partial class MainWindow : Window
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)DataContext; }
            private set { DataContext = value; }
        }

        public MainWindow()
        {
            var config = XmlLoggingConfiguration.AppConfig ?? new LoggingConfiguration();

            var observableWriter = new ObservableTextWriter();
            var textWriterTarget = new TextWriterTarget(observableWriter);
            config.AddTarget("Output", textWriterTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, textWriterTarget));

            LogManager.Configuration = config;

            Log.Info("Initializing application...");

            ServiceLocator.Initialize("RazorPad", "RazorPad.UI", "RazorPad.Core");


            var preferences = Preferences.Current;

            var themeLoader = ServiceLocator.Get<ThemeLoader>();
            var themes = themeLoader.LoadThemes(preferences.Theme);

            ViewModel = ServiceLocator.Get<MainWindowViewModel>();
            ViewModel.GetReferencesThunk = GetReferences;
            ViewModel.Messages = observableWriter;
            ViewModel.Preferences = preferences;
            ViewModel.RecentFiles = new ObservableCollection<string>(preferences.RecentFiles ?? Enumerable.Empty<string>());
            ViewModel.Themes = new ObservableCollection<Theme>(themes ?? Enumerable.Empty<Theme>());

            ViewModel.RecentFiles.CollectionChanged += (sender, args) =>
                preferences.RecentFiles = ViewModel.RecentFiles.ToArray();

            var globalNamespaceImports = preferences.GlobalNamespaceImports ?? Enumerable.Empty<string>();
            foreach (var @namespace in globalNamespaceImports)
            {
                RazorPadHost.AddGlobalImport(@namespace);
            }

            ModelProviders.DefaultFactory =
                ModelProviders.Current.GetProviderFactory(preferences.ModelProvider); ;

            CreateDemoTemplate();

            InitializeComponent();

            Log.Info("Done initializing");
        }

        private void ScrollToEnd(object sender, EventArgs args)
        {
            var textbox = (TextBox) sender;
            textbox.CaretIndex = textbox.Text.Length;
            textbox.ScrollToEnd();
        }

        void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            if (!ctrl)
                return;

            if(e.Delta > 0)
                ViewModel.FontSizeCommand.Execute("Increase");
            if(e.Delta < 0)
                ViewModel.FontSizeCommand.Execute("Decrease");
        }

        private void CreateDemoTemplate()
        {
            var demoDocument = new RazorDocument
            {
                Template = "<h1>Welcome to @Model.Name!</h1>\r\n<div>Start typing some text to get started.</div>\r\n<div>Or, try adding a property called 'Message' and see what happens...</div>\r\n\r\n<h3>@Model.Message</h3>",
                ModelProvider = new JsonModelProvider { Json = "{\r\n\tName: 'RazorPad'\r\n}" }
            };

            ViewModel.AddNewTemplateEditor(demoDocument);
        }


        private IEnumerable<string> GetReferences(IEnumerable<string> loadedReferences)
        {
            var references = loadedReferences.ToArray();

            var assemblyReferences = references.Select(s =>
                new AssemblyReference(s)
                {
                    IsOptional = true,
                    IsInstalled = true,
                });

            var dialogDataContext = new ReferencesViewModel(assemblyReferences);
            var dlg = new ReferencesDialogWindow
            {
                Owner = this,
                DataContext = dialogDataContext
            };

            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                var selectedReferences = dialogDataContext.InstalledReferences.References;
                references = selectedReferences.Select(reference => reference.Location).ToArray();
            }

            return references;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ServiceLocator.Get<IPreferencesService>().Save(Preferences.Current);
        }

        private void DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            var template = e.Document.Content as RazorTemplateEditorViewModel;

            if (template != null)
                ViewModel.TemplateEditors.Remove(template);
        }
    }
}
