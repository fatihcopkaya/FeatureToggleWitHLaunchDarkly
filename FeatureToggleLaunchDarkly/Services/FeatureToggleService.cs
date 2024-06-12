using FeatureToggleLaunchDarkly.Interfaces;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server.Interfaces;

namespace FeatureToggleLaunchDarkly.Services
{
   

    public class FeatureToggleService : IFeatureToggleService
    {
        private readonly ILdClientProvider _ldClientProvider;
        private string _flagName;

        public FeatureToggleService(ILdClientProvider ldClientProvider)
        {
            _ldClientProvider = ldClientProvider;
        }

        public void UpdateFlagName(string flagName)
        {
            _flagName = flagName;
        }

        public bool IsFeatureEnabled(string userKey, string role)
        {
            var user = Context.Builder(userKey)
                .Kind("user")
                .Set("role", role)
                .Build();

            return _ldClientProvider.GetClient().BoolVariation(_flagName, user, false);
        }
    }
}
