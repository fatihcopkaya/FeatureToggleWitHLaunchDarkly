using Consul;
using FeatureToggleLaunchDarkly.Interfaces;
using FeatureToggleLaunchDarkly.Services;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;
using System.Net.Sockets;

namespace FeatureToggleLaunchDarkly.IOC
{
    public static class IOC
    {
        public static void AddServices(this IServiceCollection services,string consulUri)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(consulUri);
            }));
            services.AddSingleton<ILdClientProvider, LdClientProvider>();
            services.AddSingleton<IFeatureToggleService, FeatureToggleService>();
            services.AddHostedService<ConsulHostedService>();
        }
    }
}

