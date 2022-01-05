using System;

namespace EasyAbp.Abp.EventBus.CAP.Models;

[Serializable]
public class EntityWrappedEto<TEntity>
{
    public TEntity Entity { get; set; }
}