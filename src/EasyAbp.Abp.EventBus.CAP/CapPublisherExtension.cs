using System;
using DotNetCore.CAP;
using Volo.Abp;

namespace EasyAbp.Abp.EventBus.CAP;

public static class CapPublisherExtension
{
    public static IDisposable UseTransaction(this ICapPublisher capPublisher, ICapTransaction capTransaction)
    {
        var previousValue = capPublisher.Transaction;
        capPublisher.Transaction = capTransaction;
        return new DisposeAction(() => capPublisher.Transaction = previousValue);
    }
}