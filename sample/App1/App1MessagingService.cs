using Microsoft.Extensions.Logging;
using SharedModule;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App1
{
    public class App1MessagingService : IApp1MessagingService
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ILogger _logger;

        public App1MessagingService(IDistributedEventBus distributedEventBus, ILoggerFactory loggerFactory)
        {
            _distributedEventBus = distributedEventBus;
            _logger = loggerFactory.CreateLogger<App1MessagingService>();
        }

        public async Task RunAsync(string message)
        {
            _logger.LogInformation($"{message} send to the App2.");

            if (!message.IsNullOrEmpty())
            {
                await _distributedEventBus.PublishAsync(new App1ToApp2TextEventData(message));
            }
            else
            {
                await _distributedEventBus.PublishAsync(new App1ToApp2TextEventData("App1 is exiting. Bye bye...!"));
            }
        }
    }
}