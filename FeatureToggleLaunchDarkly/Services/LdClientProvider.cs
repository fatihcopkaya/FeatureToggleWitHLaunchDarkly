using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;

namespace FeatureToggleLaunchDarkly.Services
{
    public class LdClientProvider : Interfaces.ILdClientProvider 
    {
        private LdClient _ldClient;
        private readonly object _lock = new object();
        public ILdClient GetClient()
        {
            return _ldClient;
        }
        // consuldeki launchapi key değişirse update edecek method
        public void UpdateClient(string sdkKey)
        {
            lock (_lock)
            {
                _ldClient?.Dispose();
                _ldClient = new LdClient(sdkKey);
            }
        }

        
    }
}
