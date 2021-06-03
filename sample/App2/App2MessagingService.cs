using Microsoft.Extensions.Logging;
using SharedModule;
using System;
using System.Linq;
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
                await _distributedEventBus.PublishAsync(new App2ToApp1TextEventData(System.Text.Encoding.Default.GetBytes(message)));
            }
            else
            {
                byte[] myByteArray = Enumerable.Repeat((byte)0x08, 1024*1024*3).ToArray();
                await _distributedEventBus.PublishAsync(new App2ToApp1TextEventData(myByteArray));
            }

        }
    }
}