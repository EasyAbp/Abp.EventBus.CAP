using Microsoft.Extensions.Logging;
using SharedModule;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    public class App2MessagingService :  IApp2MessagingService
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ILogger _logger;

        public App2MessagingService(IDistributedEventBus distributedEventBus, ILoggerFactory loggerFactory)
        {           
            _distributedEventBus = distributedEventBus;
            _logger = loggerFactory.CreateLogger<App2MessagingService>();
        }

        public async Task RunAsync(string message)
        {  
            if (!message.IsNullOrEmpty())
            {
                _logger.LogInformation($"{message} send to the App1.");
                await _distributedEventBus.PublishAsync(new App2ToApp1TextEventData(message));
            }
            else
            {
                await _distributedEventBus.PublishAsync(new App2ToApp1TextEventData("App2 is exiting. Bye bye...!"));
            }

        }
    }
}