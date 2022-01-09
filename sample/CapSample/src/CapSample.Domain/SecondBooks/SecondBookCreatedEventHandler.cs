using System;
using System.Threading.Tasks;
using CapSample.Books;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace CapSample.SecondBooks;

public class SecondBookCreatedEventHandler : IDistributedEventHandler<EntityCreatedEto<SecondBook>>, ITransientDependency
{
    private readonly IRepository<SecondBook, Guid> _repository;

    public SecondBookCreatedEventHandler(IRepository<SecondBook, Guid> repository)
    {
        _repository = repository;
    }
    
    [UnitOfWork]
    public async Task HandleEventAsync(EntityCreatedEto<SecondBook> eventData)
    {
        var book = await _repository.GetAsync(eventData.Entity.Id);
        
        book.SetEntityCreatedEventHandled();

        await _repository.UpdateAsync(book, true);
    }
}