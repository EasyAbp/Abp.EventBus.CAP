using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace CapSample.Books;

public class BookAppService : CapSampleAppService
{
    private readonly IRepository<Book, Guid> _repository;

    public BookAppService(IRepository<Book, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Book> GetAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    [UnitOfWork(IsDisabled = true)]
    public async Task<Book> CreateAsync(string name)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);
        
        var book = await _repository.InsertAsync(new Book(GuidGenerator.Create(), CurrentTenant.Id, name), true);

        await Task.Delay(TimeSpan.FromSeconds(3));
        
        var resultBook = await _repository.GetAsync(book.Id);  // EntityCreatedEventHandled should be false.
        
        await uow.CompleteAsync();

        return resultBook;
    }
}