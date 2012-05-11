using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Editors
{
    public class CodeEditor : UserControl
    {
        public static readonly DependencyProperty EditorLanguageProperty = DependencyProperty.Register("EditorLanguage",
            typeof(string), typeof(CodeEditor), new FrameworkPropertyMetadata(string.Empty, OnEditorLanguageChanged)
            {
                BindsTwoWayByDefault = true,
            });

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string), typeof(CodeEditor), new FrameworkPropertyMetadata(string.Empty, OnTextChanged)
            {
                BindsTwoWayByDefault = true,
            });

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly",
            typeof(bool), typeof(CodeEditor), new FrameworkPropertyMetadata(false, OnReadOnlyChanged)
            {
                BindsTwoWayByDefault = true,
            });


        private DispatcherTimer _textChangedTimer;
        private static readonly IDictionary<string, ICodeEditorStrategy> _strategies =
            new Dictionary<string, ICodeEditorStrategy>
                {
                    {"C#", new CSharpStrategy()},
                    {"JavaScript", new JavaScriptStrategy()},
                    {"Razor", new RazorStrategy()},
                    {"XML", new XmlStrategy()},
                };

        private FoldingManager _foldingManager;

        public TextEditor Editor { get; private set; }

        public string EditorLanguage
        {
            get { return (string)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public bool SuspendEditorUpdate { get; private set; }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public CodeEditor()
        {
            InitializeEditor();
            InitializeTextChangedTimer();
        }


        private void InitializeTextChangedTimer()
        {
            _textChangedTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _textChangedTimer.Tick += (sender, args) =>
            {
                SuspendEditorUpdate = true;
                SyncText();
                SuspendEditorUpdate = false;
            };
        }

        private void SyncText()
        {
            Text = Editor.Text;
        }

        private void InitializeEditor()
        {
            Editor = new TextEditor
                         {
                             ShowLineNumbers = true,
                             FontFamily = new FontFamily("Consolas"),
                             FontSize = (double)(new FontSizeConverter().ConvertFrom("10pt") ?? 10.0)
                         };

            Editor.KeyUp += (sender, args) =>
                                {
                                    if (_textChangedTimer.IsEnabled)
                                        _textChangedTimer.Stop();

                                    _textChangedTimer.Start();

                                };
            Content = Editor;
        }

        protected internal void InitializeFolding(AbstractFoldingStrategy foldingStrategy)
        {
            if (_foldingManager != null)
                FoldingManager.Uninstall(_foldingManager);

            var foldingManager = FoldingManager.Install(Editor.TextArea);
            foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);

            var foldingUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            foldingUpdateTimer.Tick += (o, args) => foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);

            foldingUpdateTimer.Start();

            _foldingManager = foldingManager;
        }


        static void OnReadOnlyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var codeEditor = sender as CodeEditor;
            if (codeEditor != null) 
                codeEditor.Editor.IsReadOnly = (bool)e.NewValue;
        }

        static void OnTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var codeEditor = sender as CodeEditor;

            if (codeEditor != null && !codeEditor.SuspendEditorUpdate)
                codeEditor.Editor.Text = (e.NewValue ?? string.Empty).ToString();
        }

        static void OnEditorLanguageChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var codeEditor = sender as CodeEditor;

            if (codeEditor == null)
                return;

            var strategy = _strategies[e.NewValue.ToString()];
            codeEditor.InitializeEditor();
            strategy.Apply(codeEditor);
        }
    }
}
