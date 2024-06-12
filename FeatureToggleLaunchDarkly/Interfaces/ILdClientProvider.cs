using LaunchDarkly.Sdk.Server.Interfaces;

namespace FeatureToggleLaunchDarkly.Interfaces
{
    public interface ILdClientProvider
    {
        ILdClient GetClient();
        void UpdateClient(string sdkKey);
    }
}
