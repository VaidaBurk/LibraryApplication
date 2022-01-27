using BookLibraryBackend.Models;
using BookLibraryBackend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibraryBackend.Services
{
    public class BookReader
    {
        private readonly IBookRepository _bookRepository;

        public BookReader(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        public List<Book> GetBooks()
        {
            return _bookRepository.ReadFileAndDeserialize().OrderBy(b => b.Name).ToList();
        }

        public void DisplayBooks()
        {
            List<Book> books = GetBooks();
            DisplayBooks(books);
        }

        private static void DisplayBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("No books in the library :(");
            }
            foreach (var book in books)
            {
                Console.WriteLine($"'{book.Name}' by {book.Author} ({book.Language} / {book.PublicationDate} / ISBN {book.ISBN})");
            }
        }

        public void FilterAndListBooks(string parameter, string searchString)
        {
            switch (parameter)
            {
                case "a":
                    List<Book> books = GetBooks().Where(b => b.Author.ToLower().Contains(searchString)).ToList();
                    DisplayBooks(books);
                    break;
                case "c":
                    books = GetBooks().Where(b => b.Category.ToLower().Contains(searchString)).ToList();
                    DisplayBooks(books);
                    break;
                case "l":
                    books = GetBooks().Where(b => b.Language.ToLower() == searchString).ToList();
                    DisplayBooks(books);
                    break;
                case "i":
                    books = GetBooks().Where(b => b.ISBN.Contains(searchString)).ToList();
                    DisplayBooks(books);
                    break;
                case "n":
                    books = GetBooks().Where(b => b.Name.ToLower().Contains(searchString)).ToList();
                    DisplayBooks(books);
                    break;
                case "av":
                    books = GetBooks().Where(b => b.IsBookTaken == Convert.ToBoolean(searchString)).ToList();
                    DisplayBooks(books);
                    break;
                default:
                    Console.WriteLine("Wrong parameter. Try another one.");
                    break;
            }
        }
    }
}
