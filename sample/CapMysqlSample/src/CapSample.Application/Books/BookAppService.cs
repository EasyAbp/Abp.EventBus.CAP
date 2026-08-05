using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace CapSample.Books;

public class BookAppService : CapSampleAppService
{
    private readonly IRepository<Book, Guid> _repository;
    private readonly IDistributedEventBus _distributedEventBus;

    public BookAppService(
        IRepository<Book, Guid> repository, IDistributedEventBus distributedEventBus)
    {
        _repository = repository;
        _distributedEventBus = distributedEventBus;
    }

    public async Task<Book> GetAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }


    public virtual async Task<Book> CreateAsync(string name)
    {
        var book = await _repository.InsertAsync(new Book(GuidGenerator.Create(), CurrentTenant.Id, name));
        await _distributedEventBus.PublishAsync(name);
        return book;
    }
}