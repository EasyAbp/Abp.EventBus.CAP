using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus;

namespace EasyAbp.Abp.EventBus.CAP
{
    public interface ICapDistributedEventHandler<in TEvent> : IEventHandler
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// </summary>
        /// <param name="eventData">Event data</param>
        /// <param name="headers">Cap Headers</param>
        /// <returns></returns>
        Task HandleEventAsync(TEvent eventData, [FromCap] CapHeader headers);
    }
}
