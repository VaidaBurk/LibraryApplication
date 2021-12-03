using BookLibraryBackend.Models;
using BookLibraryBackend.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookLibraryBackend.Services
{
    public class BookService
    {
        public readonly IBookRepository bookRepository;
        private static readonly string _filePath = "books.json";

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public List<Book> GetBooks()
        {
            if (!File.Exists(_filePath))
            {
                var jsonFile = File.Create(_filePath);
                jsonFile.Close();
                List<Book> books = new();
                bookRepository.WriteToFile(books, _filePath);
            }
            return bookRepository.ReadFileAndDeserialize(_filePath).OrderBy(b => b.Name).ToList();
        }

        public Book GetBookByIsbn(string isbn, List<Book> books)
        {
            return books.Where(b => b.ISBN == isbn).FirstOrDefault();
        }

        public void AddNewBook()
        {
            Console.WriteLine("Enter book name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter book author:");
            string author = Console.ReadLine();
            Console.WriteLine("Enter book category:");
            string category = Console.ReadLine();
            Console.WriteLine("Enter book language:");
            string language = Console.ReadLine();
            Console.WriteLine("Enter publication date:");
            string publicationDate = Console.ReadLine();
            Console.WriteLine("Enter ISBN:");
            string isbn = CheckIsbn(Console.ReadLine());
            Console.WriteLine("Do you want to save the book? (y / n)");
            string answer = Console.ReadLine();
            if (answer == "y")
            {
                Book book = new()
                {
                    Name = name,
                    Author = author,
                    Category = category,
                    Language = language,
                    PublicationDate = publicationDate,
                    ISBN = isbn
                };
                List<Book> books = GetBooks();
                books.Add(book);
                bookRepository.WriteToFile(books, _filePath);
                Console.WriteLine($"Book '{name}' is added!");
            }
        }

        public void DeleteBook()
        {
            Console.WriteLine("Enter book ISBN:");
            string isbn = CheckIsbn(Console.ReadLine());
            List<Book> books = GetBooks();
            Book book = GetBookByIsbn(isbn, books);
            if (book != null)
            {
                GetBooks().Remove(book);
                bookRepository.WriteToFile(books, _filePath);
                Console.WriteLine("Book is removed.");
            }
            else
            {
                Console.WriteLine("Book was not found.");
            }
        }

        public string CheckIsbn(string isbn)
        {
            if (IsIsbnValid(isbn) == false)
            {
                Console.WriteLine("Wrong ISBN. ISBN should contain 13 numbers. Try again.");
                isbn = Console.ReadLine();
                CheckIsbn(isbn);
            }
            return isbn;
        }

        public bool IsIsbnValid(string isbn)
        {
            if (isbn.Length == 13)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ListAllBooks()
        {
            List<Book> books = GetBooks();
            if (books.Count == 0)
            {
                Console.WriteLine("No books in the library :(");
            }
            foreach (var book in books)
            {
                Console.WriteLine($"'{book.Name}' by {book.Author} ({book.Language} / {book.PublicationDate} / ISBN {book.ISBN})");
            }
        }

        public void FilterAndListBooks()
        {
            Console.WriteLine("Enter filtering parameter: a - by author; c - by category; l - by language; i - by ISBN; n - by name; av - available books");
            string filterParameter = Console.ReadLine();

            switch (filterParameter)
            {
                case "a":
                    Console.WriteLine("Enter author name:");
                    string author = Console.ReadLine();
                    List<Book> books = GetBooks().Where(b => b.Author.Contains(author)).ToList();
                    ListAllBooks();
                    break;
                case "c":
                    Console.WriteLine("Enter category:");
                    string category = Console.ReadLine();
                    books = GetBooks().Where(b => b.Category.Contains(category)).ToList();
                    ListAllBooks();
                    break;
                case "l":
                    Console.WriteLine("Enter language code (XX):");
                    string language = Console.ReadLine();
                    books = GetBooks().Where(b => b.Language == language).ToList();
                    ListAllBooks();
                    break;
                case "i":
                    Console.WriteLine("Enter ISBN:");
                    string isbn = Console.ReadLine();
                    books = GetBooks().Where(b => b.ISBN.Contains(isbn)).ToList();
                    ListAllBooks();
                    break;
                case "n":
                    Console.WriteLine("Enter book name:");
                    string name = Console.ReadLine();
                    books = GetBooks().Where(b => b.Name.Contains(name)).ToList();
                    ListAllBooks();
                    break;
                case "av":
                    books = GetBooks().Where(b => b.IsBookTaken == false).ToList();
                    ListAllBooks();
                    break;
                default:
                    Console.WriteLine("Wrong parameter. Try another one.");
                    break;
            }
        }
    }
}
