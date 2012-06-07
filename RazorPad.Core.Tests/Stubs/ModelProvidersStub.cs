using RazorPad.Providers;

namespace RazorPad.Core.Tests.Stubs
{
    public class ModelProvidersStub : ModelProviders
    {
        public ModelProvidersStub(params IModelProviderFactory[] factories) 
            : base(factories ?? new [] { new BasicModelProvider.BasicModelProviderFactory(), })
        {
        }
    }
}
