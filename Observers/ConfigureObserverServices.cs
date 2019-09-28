using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Observers
{
    public static class ConfigureObserverServices
    {
        public static IServiceCollection AddObservers(this IServiceCollection services, IConfigurationSection observersConfig)
        {
            var observerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
               .Where(x => typeof(Observer).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            var addHostedServiceMethod = typeof(ServiceCollectionHostedServiceExtensions)
                .GetMethod(nameof(ServiceCollectionHostedServiceExtensions.AddHostedService), new[] { typeof(IServiceCollection) });

            var addSingletonMethod = typeof(ServiceCollectionServiceExtensions)
                .GetMethod(nameof(ServiceCollectionServiceExtensions.AddSingleton), new[] { typeof(IServiceCollection), typeof(Type), typeof(object) });

            foreach (var observerType in observerTypes)
            {
                addHostedServiceMethod
                    .MakeGenericMethod(observerType)
                    .Invoke(null, new[] { services });

                var settingsType = observerType.GetNestedType("Settings");
                var config = observersConfig.GetSection(observerType.Name);

                addSingletonMethod
                    .Invoke(null, new object[] { services, settingsType, config.Get(settingsType) });
            }

            return services;
        }
    }
}
