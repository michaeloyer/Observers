using Serilog;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Observers
{
    public class TimerObserver : Observer
    {
        private readonly Settings settings;
        private readonly ILogger logger;

        public TimerObserver(Settings settings, ILogger logger, CoreSettings coreSettings) : base(coreSettings, logger)
        {
            this.settings = settings;
            this.logger = logger.ForContext<TimerObserver>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var stopWatch = Stopwatch.StartNew();

            while (stoppingToken.IsCancellationRequested == false)
            {
                logger.Information("Time Passed: {timePassed}", stopWatch.Elapsed.ToString(@"hh\:mm\:ss"));
                await Task.Delay(TimeSpan.FromSeconds(settings.Seconds), stoppingToken);
            }
        }

        public class Settings
        {
            public int Seconds { get; set; }
        }
    }
}
