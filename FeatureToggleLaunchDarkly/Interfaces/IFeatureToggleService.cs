namespace FeatureToggleLaunchDarkly.Interfaces
{
    public interface IFeatureToggleService
    {
        bool IsFeatureEnabled(string userKey, string role);
        void UpdateFlagName(string flagName);
    }
}
