using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace CapSample.SecondBooks;

public class SecondBook : AggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    public virtual string Name { get; protected set; }
    
    public virtual bool EntityCreatedEventHandled { get; protected set; }

    protected SecondBook()
    {
    }

    public SecondBook(Guid id, Guid? tenantId, string name) : base(id)
    {
        TenantId = tenantId;
        Name = name;
    }

    public void SetEntityCreatedEventHandled()
    {
        EntityCreatedEventHandled = true;
    }
}