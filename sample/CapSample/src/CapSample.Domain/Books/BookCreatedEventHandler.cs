using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace CapSample.Books;

public class BookCreatedEventHandler : IDistributedEventHandler<EntityCreatedEto<Book>>, ITransientDependency
{
    private readonly IRepository<Book, Guid> _repository;

    public BookCreatedEventHandler(IRepository<Book, Guid> repository)
    {
        _repository = repository;
    }
    
    [UnitOfWork(true)]
    public virtual async Task HandleEventAsync(EntityCreatedEto<Book> eventData)
    {
        var book = await _repository.GetAsync(eventData.Entity.Id);
        
        book.SetEntityCreatedEventHandled();

        await _repository.UpdateAsync(book, true);
    }
}