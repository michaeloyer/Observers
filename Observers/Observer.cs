using Microsoft.Extensions.Hosting;
using Serilog;

namespace Observers
{
    public abstract class Observer : BackgroundService
    {
        private readonly CoreSettings settings;

        public Observer(CoreSettings settings, ILogger logger)
        {
            this.settings = settings;

            logger.Information(settings.Message);
        }
    }
}
