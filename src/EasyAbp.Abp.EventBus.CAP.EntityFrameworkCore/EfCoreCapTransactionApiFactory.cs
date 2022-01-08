using System;
using DotNetCore.CAP;
using EasyAbp.Abp.EventBus.CAP;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Uow.EntityFrameworkCore;

namespace EasyAbp.Abp.EventBus.Cap;

public class EfCoreCapTransactionApiFactory : ICapTransactionApiFactory
{
    public Type TransactionApiType { get; } = typeof(EfCoreTransactionApi);

    protected readonly ICapPublisher Publisher;
    protected readonly AbpEfCoreDbContextCapOptions Options;
    protected readonly ICapDbProviderInfoProvider CapDbProviderInfoProvider;
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    public EfCoreCapTransactionApiFactory(
        ICapPublisher publisher,
        IOptions<AbpEfCoreDbContextCapOptions> options,
        ICapDbProviderInfoProvider capDbProviderInfoProvider,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        
        Publisher = publisher;
        Options = options.Value;
        CapDbProviderInfoProvider = capDbProviderInfoProvider;
        CancellationTokenProvider = cancellationTokenProvider;
    }
    
    public virtual ITransactionApi Create(ITransactionApi originalApi)
    {
        var efApi = (EfCoreTransactionApi)originalApi;

        var capTrans = CreateCapTransactionOrNull(efApi);

        return capTrans is null
            ? originalApi
            : new EfCoreTransactionApi(capTrans, efApi.StarterDbContext, CancellationTokenProvider);
    }

    protected virtual IDbContextTransaction CreateCapTransactionOrNull(EfCoreTransactionApi originalApi)
    {
        if (originalApi.StarterDbContext is null || Options.AbpEfDbContextType is null ||
            originalApi.StarterDbContext.GetType() != Options.AbpEfDbContextType)
        {
            return null;
        }

        var dbProviderInfo = CapDbProviderInfoProvider.GetOrNull(originalApi.StarterDbContext.Database.ProviderName);
        
        if (dbProviderInfo?.CapTransactionType is null || dbProviderInfo.CapEfDbTransactionType is null)
        {
            return null;
        }
        
        var capTransactionType = dbProviderInfo.CapTransactionType;

        if (ActivatorUtilities.CreateInstance(Publisher.ServiceProvider, capTransactionType) is not ICapTransaction capTransaction)
        {
            return null;
        }
        
        capTransaction.DbTransaction = originalApi.DbContextTransaction;
        capTransaction.AutoCommit = false;

        Publisher.Transaction.Value = capTransaction;

        return (IDbContextTransaction)Activator.CreateInstance(dbProviderInfo.CapEfDbTransactionType, capTransaction);
    }
}