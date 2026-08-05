using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace CapSample.Books;

public class Book : AggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    public virtual string Name { get; protected set; }
    
    public virtual bool EntityCreatedEventHandled { get; protected set; }

    protected Book()
    {
    }

    public Book(Guid id, Guid? tenantId, string name) : base(id)
    {
        TenantId = tenantId;
        Name = name;
    }

    public void SetEntityCreatedEventHandled()
    {
        EntityCreatedEventHandled = true;
    }
}