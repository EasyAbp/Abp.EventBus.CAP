using System;
using CapSample.SecondBooks;

namespace CapSample.Books;

[Serializable]
public class BookAndSecondBook
{
    public Book Book { get; set; }
    
    public SecondBook SecondBook { get; set; }

    public BookAndSecondBook()
    {
        
    }
    
    public BookAndSecondBook(Book book, SecondBook secondBook)
    {
        Book = book;
        SecondBook = secondBook;
    }
}