using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorPad.Persistence;
using RazorPad.UI.Settings;
using RazorPad.UI.Theming;
using RazorPad.ViewModels;

namespace RazorPad.UI
{
    [TestClass]
    public class ServiceLocatorTests
    {
        [Export] public Preferences Preferences = Preferences.Default;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ServiceLocator.Initialize(
                "RazorPad.Core", 
                "RazorPad.UI", 
                "RazorPad.UI.Tests"
            );
        }

        [TestMethod]
        public void ShouldLoadRazorDocumentSources()
        {
            var sources = ServiceLocator.GetMany<IRazorDocumentSource>();
            Assert.IsNotNull(sources);
            Assert.AreNotEqual(0, sources.Count());
        }

        [TestMethod]
        public void ShouldLoadXmlRazorDocumentSource()
        {
            var sources = ServiceLocator.GetMany<IRazorDocumentSource>();
            var xmlDocumentSource = sources.OfType<XmlRazorDocumentSource>();
            Assert.AreNotEqual(0, xmlDocumentSource.Count());
        }

        [TestMethod]
        public void ShouldLoadRazorDocumentManager()
        {
            var documentManager = ServiceLocator.Get<RazorDocumentManager>();
            Assert.IsNotNull(documentManager);
        }

        [TestMethod]
        public void ShouldLoadMainWindowViewModel()
        {
            var viewModel = ServiceLocator.Get<MainViewModel>();
            Assert.IsNotNull(viewModel);
        }

        [TestMethod]
        public void ShouldLoadThemeLoader()
        {
            var loader = ServiceLocator.Get<ThemeLoader>();
            Assert.IsNotNull(loader);
        }

        [TestMethod]
        public void ShouldLoadPrefrencesService()
        {
            var service = ServiceLocator.Get<IPreferencesService>();
            Assert.IsNotNull(service);
        }
    }
}
