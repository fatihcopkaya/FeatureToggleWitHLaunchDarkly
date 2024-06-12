using Consul;
using FeatureToggleLaunchDarkly.Interfaces;
using LaunchDarkly.Sdk.Server.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeatureToggleLaunchDarkly.Services
{
    public class ConsulHostedService : IHostedService, IDisposable
    {
        private readonly IConsulClient _consulClient;
        private readonly ILdClientProvider _ldClientProvider;
        private readonly IFeatureToggleService _featureToggleService;
        private Timer _timer;
        private string _sdkKey;
        private string _flagName;

        public ConsulHostedService(IConsulClient consulClient, ILdClientProvider ldClientProvider, IFeatureToggleService featureToggleService)
        {
            _consulClient = consulClient;
            _ldClientProvider = ldClientProvider;
            _featureToggleService = featureToggleService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitialUpdateSettings(); // ilk başlatmada Consul'dan değerleri al
            _timer = new Timer(UpdateSettings, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5)); // her 5 dakikada bir consuldeki değerler değiştirilirse uygulamaya dahil etmek için çalış.
        }

        private async Task InitialUpdateSettings()
        {
            var sdkKeyKv = await _consulClient.KV.Get("config/launchdarkly/sdkKey");
            var flagNameKv = await _consulClient.KV.Get("config/launchdarkly/flagName");

            if (sdkKeyKv.Response != null && flagNameKv.Response != null)
            {
                _sdkKey = Encoding.UTF8.GetString(sdkKeyKv.Response.Value);
                _flagName = Encoding.UTF8.GetString(flagNameKv.Response.Value);

                _ldClientProvider.UpdateClient(_sdkKey);
                _featureToggleService.UpdateFlagName(_flagName);
            }
        }

        private async void UpdateSettings(object state)
        {
            var sdkKeyKv = await _consulClient.KV.Get("config/launchdarkly/sdkKey");
            var flagNameKv = await _consulClient.KV.Get("config/launchdarkly/flagName");

            if (sdkKeyKv.Response != null && flagNameKv.Response != null)
            {
                var newSdkKey = Encoding.UTF8.GetString(sdkKeyKv.Response.Value);
                var newFlagName = Encoding.UTF8.GetString(flagNameKv.Response.Value);

                if (_sdkKey != newSdkKey || _flagName != newFlagName)
                {
                    _sdkKey = newSdkKey;
                    _flagName = newFlagName;

                    _ldClientProvider.UpdateClient(_sdkKey);
                    _featureToggleService.UpdateFlagName(_flagName);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
