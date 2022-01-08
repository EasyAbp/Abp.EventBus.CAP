using System;
using Volo.Abp.Uow;

namespace EasyAbp.Abp.EventBus.Cap;

public interface ICapTransactionApiFactory
{
    Type TransactionApiType { get; }
    
    ITransactionApi Create(ITransactionApi originalApi);
}