using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;

namespace Observers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseSerilog((host, builder) => builder.WriteTo.Console())
                .ConfigureServices((host, services) =>
                {
                    var config = host.Configuration;
                    services.AddSingleton(config.GetSection("Observers").Get<CoreSettings>());
                    services.AddHostedService<TimerObserver>()
                        .AddSingleton(host.Configuration.GetSection("TimerObserver").Get<TimerObserver.Settings>());
                })
                .RunConsoleAsync();
        }
    }
}
