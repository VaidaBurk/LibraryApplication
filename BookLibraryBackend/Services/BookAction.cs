using BookLibraryBackend.Models;
using BookLibraryBackend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibraryBackend.Services
{
    public class BookAction
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookReader _bookReader;

        public BookAction(IBookRepository bookRepository, BookReader bookService)
        {
            _bookRepository = bookRepository;
            _bookReader = bookService;
        }

        private readonly int maxBorrowingPeriod = 61;

        public void AddNewBook(string isbn, string name, string author, string publicationDate, string category, string language) // TODO: publicationDate should be int type
        {
            if (IsIsbnFormatValid(isbn))
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
                // TODO: should there be book id and if yes, should we generate random UUID to newly added book (to make it unique)
                List<Book> books = _bookReader.GetBooks();
                books.Add(book);
                _bookRepository.WriteToFile(books);
                Console.WriteLine($"Book '{name}' is added!");
            }
            else
            {
                Console.WriteLine("Wrong ISBN format.");
            }
        }

        public void DeleteBook(string isbn)
        {
            if (IsIsbnFormatValid(isbn))
            {
                List<Book> books = _bookReader.GetBooks();
                Book book = books.Where(b => b.ISBN == isbn).FirstOrDefault();
                if (book != null)
                {
                    _bookReader.GetBooks().Remove(book);
                    _bookRepository.WriteToFile(books);
                    Console.WriteLine("Book is removed.");
                }
                else
                {
                    Console.WriteLine("Book was not found.");
                }
            }
            else
            {
                Console.WriteLine("Wrong ISBN format.");
            }
        }

        public void TakeBook(string isbn, int readerId, int period)
        {
            if (IsIsbnFormatValid(isbn))
            {
                if (IsBookAvailable(isbn))
                {
                    if (IsReaderBasketFull(readerId) == false)
                    {
                        if (period <= maxBorrowingPeriod)
                        {
                            ConfirmBookBorrowing(isbn, readerId, period);
                            Console.WriteLine($"Please return the book before {DateTime.Now.AddDays(period)}. Happy reading!");
                        }
                        if (period > maxBorrowingPeriod)
                        {
                            ConfirmBookBorrowing(isbn, readerId, maxBorrowingPeriod);
                            Console.WriteLine($"Borrowing for {period} days is not available. Period was set to max - 2 months. Please return the book before {DateTime.Now.AddDays(period)}. Happy reading!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry, you already have 3 books in your basket.");
                    }
                }
                else
                {
                    Console.WriteLine("Book is not available.");
                }
            }
            else
            {
                Console.WriteLine("Wrong ISBN format.");
            }
        }

        public void ReturnBook(string isbn, int readerId)
        {
            if (IsIsbnFormatValid(isbn))
            {
                List<Book> books = _bookReader.GetBooks();
                Book selectedBook = books.Where(b => b.ISBN == isbn).FirstOrDefault();
                if (selectedBook != null)
                {
                    if (selectedBook.ReaderId == readerId)
                    {
                        if (selectedBook.ReturnDeadline < DateTime.Now)
                        {
                            Console.WriteLine("You are late. Spend more time for reading next time!");
                        }
                        selectedBook.IsBookTaken = false;
                        selectedBook.ReaderId = null;
                        selectedBook.ReturnDeadline = null;

                        _bookRepository.WriteToFile(books);
                        Console.WriteLine("Book was returned!");
                    }
                    else
                    {
                        Console.WriteLine("This book is not in reader's basket.");
                    }
                }
                else
                {
                    Console.WriteLine("Book was not found.");
                }
            }
            else
            {
                Console.WriteLine("Wrong ISBN format.");
            }
        }

        private void ConfirmBookBorrowing(string isbn, int readerId, int period)
        {
            List<Book> books = _bookReader.GetBooks();
            Book selectedBook = books.Where(b => b.ISBN == isbn).FirstOrDefault();

            selectedBook.IsBookTaken = true;
            selectedBook.ReaderId = readerId;
            selectedBook.ReturnDeadline = DateTime.Now.AddDays(period);

            _bookRepository.WriteToFile(books);
        }

        private static bool IsIsbnFormatValid(string isbn)
        {
            return isbn.Length == 13;
        }

        private bool IsBookAvailable(string isbn)
        {
            List<Book> books = _bookReader.GetBooks();
            if (books.Select(b => b.ISBN).Contains(isbn))
            {
                if (books.Where(b => b.ISBN == isbn).FirstOrDefault().IsBookTaken == false)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool IsReaderBasketFull(int readerId)
        {
            List<Book> books = _bookReader.GetBooks();
            int numberOfBorrowedBooks = books.Where(b => b.ReaderId == readerId).Count();
            if (numberOfBorrowedBooks < 3)
            {
                return false;
            }
            return true;
        }
    }
}
