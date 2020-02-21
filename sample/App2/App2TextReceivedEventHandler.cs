using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    /// <summary>
    /// Used to know when App1 has received a message sent by App2.
    /// </summary>
    public class App2TextReceivedEventHandler : IDistributedEventHandler<App1TextReceivedEventData>, ITransientDependency
    {
        private readonly ILogger _logger;

        public App2TextReceivedEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App2TextEventHandler>();
        }

        public Task HandleEventAsync(App1TextReceivedEventData eventData)
        {
            _logger.LogInformation("--------> App1 has received the message: " + eventData.ReceivedText.TruncateWithPostfix(32));
            return Task.CompletedTask;
        }
    }
}
