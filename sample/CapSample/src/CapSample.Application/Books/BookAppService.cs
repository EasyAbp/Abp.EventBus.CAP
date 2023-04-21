using System;
using System.Threading.Tasks;
using CapSample.SecondBooks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace CapSample.Books;

public class BookAppService : CapSampleAppService
{
    private readonly IRepository<Book, Guid> _repository;
    private readonly IRepository<SecondBook, Guid> _secondBookRepository;

    public BookAppService(
        IRepository<Book, Guid> repository,
        IRepository<SecondBook, Guid> secondBookRepository)
    {
        _repository = repository;
        _secondBookRepository = secondBookRepository;
    }

    public async Task<Book> GetAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    [UnitOfWork(IsDisabled = true)]
    public virtual async Task<Book> CreateAsync(string name)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);
        
        var book = await _repository.InsertAsync(new Book(GuidGenerator.Create(), CurrentTenant.Id, name), true);

        await Task.Delay(TimeSpan.FromSeconds(3));
        
        var resultBook = await _repository.GetAsync(book.Id);  // EntityCreatedEventHandled should be false.
        
        await uow.CompleteAsync();

        return resultBook;
    }

    [UnitOfWork(IsDisabled = true)]
    public virtual async Task<BookAndSecondBook> CreateWithSecondBookAsync(string name)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);
        
        var book = await _repository.InsertAsync(new Book(GuidGenerator.Create(), CurrentTenant.Id, name), true);
        var secondBook = await _secondBookRepository.InsertAsync(new SecondBook(GuidGenerator.Create(), CurrentTenant.Id, name), true);

        await Task.Delay(TimeSpan.FromSeconds(3));
        
        var resultBook = await _repository.GetAsync(book.Id);  // EntityCreatedEventHandled should be false.
        var resultSecondBook = await _secondBookRepository.GetAsync(secondBook.Id);  // EntityCreatedEventHandled should be false.
        
        await uow.CompleteAsync();

        return new BookAndSecondBook(resultBook, resultSecondBook);
    }
}