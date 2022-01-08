using System;

namespace EasyAbp.Abp.EventBus.Cap;

public class CapDbProviderInfo
{
    public Type CapTransactionType { get; }
    
    public Type CapEfDbTransactionType { get; }
    
    public CapDbProviderInfo(string capTransactionTypeName, string capEfDbTransactionTypeName)
    {
        CapTransactionType = Type.GetType(capTransactionTypeName, false);
        CapEfDbTransactionType = Type.GetType(capEfDbTransactionTypeName, false);
    }
}