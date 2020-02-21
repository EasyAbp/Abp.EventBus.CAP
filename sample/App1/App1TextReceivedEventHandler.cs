using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App1
{
    /// <summary>
    /// Used to know when App2 has received a message sent by App1.
    /// </summary>
    public class App1TextReceivedEventHandler : IDistributedEventHandler<App2TextReceivedEventData>, ITransientDependency
    {
        private readonly ILogger _logger;

        public App1TextReceivedEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App1TextEventHandler>();
        }

        public Task HandleEventAsync(App2TextReceivedEventData eventData)
        {
            _logger.LogInformation("--------> App2 has received the message: " + eventData.ReceivedText.TruncateWithPostfix(32));

            return Task.CompletedTask;
        }
    }
}
