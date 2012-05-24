using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorPad.ViewModels;

namespace RazorPad.UI.AssemblyReference
{
    [TestClass]
    public class StandardReferencesLocatorTests
    {
        [TestMethod]
        public void ShouldLocateStandardDotNetReferences()
        {
            var locator = new StandardReferencesLocator();
            
            var paths = locator.GetStandardReferences();

            Assert.AreNotEqual(0, paths.Count());
        }
    }
}
