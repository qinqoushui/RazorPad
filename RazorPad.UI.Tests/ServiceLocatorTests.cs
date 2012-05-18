using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorPad.Persistence;
using RazorPad.ViewModels;

namespace RazorPad.UI
{
    [TestClass]
    public class ServiceLocatorTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ServiceLocator.Initialize("RazorPad.UI", "RazorPad.Core");
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
            var viewModel = ServiceLocator.Get<MainWindowViewModel>();
            Assert.IsNotNull(viewModel);
        }
    }
}
