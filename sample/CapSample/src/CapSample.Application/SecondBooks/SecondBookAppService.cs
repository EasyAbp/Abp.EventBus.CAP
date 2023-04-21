using System;
using System.Threading.Tasks;
using CapSample.Books;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace CapSample.SecondBooks;

public class SecondBookAppService : CapSampleAppService
{
    private readonly IRepository<SecondBook, Guid> _repository;

    public SecondBookAppService(IRepository<SecondBook, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<SecondBook> GetAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    [UnitOfWork(IsDisabled = true)]
    public virtual async Task<SecondBook> CreateAsync(string name)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);
        
        var book = await _repository.InsertAsync(new SecondBook(GuidGenerator.Create(), CurrentTenant.Id, name), true);

        await Task.Delay(TimeSpan.FromSeconds(3));
        
        var resultBook = await _repository.GetAsync(book.Id);  // EntityCreatedEventHandled should be false.
        
        await uow.CompleteAsync();

        return resultBook;
    }
}