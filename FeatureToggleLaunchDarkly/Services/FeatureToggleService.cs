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
        //consuldeki flag değeri değişirse update edecek method
        public void UpdateFlagName(string flagName)
        {
            _flagName = flagName;
        }
        // role için launchdarkly de belirttiğimiz flag ve segment değerlerine uygunsa true döndüren method
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
