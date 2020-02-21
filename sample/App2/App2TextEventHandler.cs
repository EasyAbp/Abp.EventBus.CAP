using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    /// <summary>
    /// Used to listen messages sent to App2 by App1.
    /// </summary>
    public class App2TextEventHandler : IDistributedEventHandler<App1ToApp2TextEventData>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ILogger _logger;

        public App2TextEventHandler(IDistributedEventBus distributedEventBus, ILoggerFactory loggerFactory)
        {
            _distributedEventBus = distributedEventBus;
            _logger = loggerFactory.CreateLogger<App2TextEventHandler>();

        }

        public async Task HandleEventAsync(App1ToApp2TextEventData eventData)
        {
            _logger.LogInformation("************************ INCOMING MESSAGE ****************************");
            _logger.LogInformation(eventData.TextMessage);
            _logger.LogInformation("**********************************************************************");
 
            await _distributedEventBus.PublishAsync(new App2TextReceivedEventData(eventData.TextMessage));
        }
    }
}
