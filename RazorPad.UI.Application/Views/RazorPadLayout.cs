using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using RazorPad.ViewModels;

namespace RazorPad.Views
{
    public class RazorPadLayout : DataTemplateSelector
    {
        private readonly IDictionary<string, DataTemplate> _templates;

        public DataTemplate BrowserViewTemplate
        {
            get { return _templates["BrowserView"]; }
            set { _templates["BrowserView"] = value; }
        }

        public DataTemplate DocumentEditorTemplate
        {
            get { return _templates["DocumentEditor"]; }
            set { _templates["DocumentEditor"] = value; }
        }

        public DataTemplate ErrorsTemplate
        {
            get { return _templates["Errors"]; }
            set { _templates["Errors"] = value; }
        }

        public DataTemplate GeneratedCodeTemplate
        {
            get { return _templates["GeneratedCode"]; }
            set { _templates["GeneratedCode"] = value; }
        }

        public DataTemplate MessagesTemplate
        {
            get { return _templates["Messages"]; }
            set { _templates["Messages"] = value; }
        }

        public DataTemplate ModelBuilderTemplate
        {
            get { return _templates["ModelBuilder"]; }
            set { _templates["ModelBuilder"] = value; }
        }

        public DataTemplate SourceViewTemplate
        {
            get { return _templates["SourceView"]; }
            set { _templates["SourceView"] = value; }
        }

        public DataTemplate SyntaxTreeTemplate
        {
            get { return _templates["SyntaxTree"]; }
            set { _templates["SyntaxTree"] = value; }
        }

        public string LayoutFilename { get; set; }


        public RazorPadLayout()
        {
            _templates = new Dictionary<string, DataTemplate>();
            LayoutFilename = @".\layout.xml";
        }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is RazorTemplateEditorViewModel)
                return DocumentEditorTemplate;

            return base.SelectTemplate(item, container);
        }

        public void Deserialize(DockingManager dockingManager, LayoutRoot defaultLayout = null)
        {
            var layoutSerializer = new XmlLayoutSerializer(dockingManager);
            layoutSerializer.LayoutSerializationCallback += Deserialize;

            if (File.Exists(LayoutFilename))
            {
                layoutSerializer.Deserialize(LayoutFilename);
            }
            else if(defaultLayout != null)
            {
                layoutSerializer.Deserialize(new StringReader((string) defaultLayout));
            }
        }

        private void Deserialize(object sender, LayoutSerializationCallbackEventArgs e)
        {
            var contentId = e.Model.ContentId;

            if (string.IsNullOrWhiteSpace(contentId) || !_templates.ContainsKey(contentId)) 
                return;

            var template = _templates[contentId];
            e.Content = template.LoadContent();
        }

        public void Serialize(DockingManager dockingManager)
        {
            using(var layout = File.Open(LayoutFilename, FileMode.Create))
            {
                new XmlLayoutSerializer(dockingManager).Serialize(layout);
                layout.Flush();
            }
        }
    }
}
