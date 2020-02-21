using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App1
{
    /// <summary>
    /// Used to listen messages sent to App2 by App1.
    /// </summary>
    public class App1TextEventHandler : IDistributedEventHandler<App2ToApp1TextEventData>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ILogger _logger;

        public App1TextEventHandler(IDistributedEventBus distributedEventBus, ILoggerFactory loggerFactory)
        {
            _distributedEventBus = distributedEventBus;
            _logger = loggerFactory.CreateLogger<App1TextEventHandler>();

        }

        public async Task HandleEventAsync(App2ToApp1TextEventData eventData)
        {
            _logger.LogInformation("************************ INCOMING MESSAGE ****************************");
            _logger.LogInformation(eventData.TextMessage);
            _logger.LogInformation("**********************************************************************");

           await _distributedEventBus.PublishAsync(new App1TextReceivedEventData(eventData.TextMessage));
        }
    }
}
