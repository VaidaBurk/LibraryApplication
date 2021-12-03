using BookLibraryBackend.Models;
using BookLibraryBackend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibraryBackend.Services
{
    class BorrowingService
    {
        public readonly IBookRepository bookRepository;
        private readonly BookService _bookService;
        private static readonly string _filePath = "books.json";

        public BorrowingService(IBookRepository bookRepository, BookService bookService)
        {
            this.bookRepository = bookRepository;
            _bookService = bookService;
        }

        public void TakeBook()
        {
            Console.WriteLine("Enter book ISBN:");
            string isbn = _bookService.CheckIsbn(Console.ReadLine());
            if (IsBookAvailable(isbn) == true)
            {
                Console.WriteLine("Enter Reader ID:");
                int readerId = Int32.Parse(Console.ReadLine());
                if (IsReaderBasketFull(readerId) == false)
                {
                    Console.WriteLine("How many days would you like to keep a book?");
                    int period = Int32.Parse(Console.ReadLine());
                    period = CheckBorrowingPeriodAvailability(period);

                    List<Book> books = _bookService.GetBooks();
                    Book selectedBook = _bookService.GetBookByIsbn(isbn, books);

                    selectedBook.IsBookTaken = true;
                    selectedBook.ReaderId = readerId;
                    selectedBook.ReturnDeadline = DateTime.Now.AddDays(period);

                    bookRepository.WriteToFile(books, _filePath);
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

        public void ReturnBook()
        {
            Console.WriteLine("Enter book ISBN:");
            string isbn = _bookService.CheckIsbn(Console.ReadLine());
            Console.WriteLine("Enter Reader ID:");
            int readerId = Int32.Parse(Console.ReadLine());

            List<Book> books = _bookService.GetBooks();
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

                    bookRepository.WriteToFile(books, _filePath);
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

        public bool IsBookAvailable(string isbn)
        {
            List<Book> books = _bookService.GetBooks();
            if (books.Select(b => b.ISBN).Contains(isbn))
            {
                if (_bookService.GetBookByIsbn(isbn, books).IsBookTaken == false)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool IsReaderBasketFull(int readerId)
        {
            List<Book> books = _bookService.GetBooks();
            int numberOfBorrowedBooks = books.Where(b => b.ReaderId == readerId).Count();
            if (numberOfBorrowedBooks < 3)
            {
                return false;
            }
            return true;
        }

        public int CheckBorrowingPeriodAvailability(int period)
        {
            if (period <= 61)
            {
                ConfirmBorrowingPeriodAvailability(period);
            }
            else
            {
                AskToChooseShorterPeriod();
            }
            return period;
        }

        public static void ConfirmBorrowingPeriodAvailability(int period)
        {
            DateTime pickUpTime = DateTime.Now;
            DateTime returnDeadline = pickUpTime.AddDays(period);
            Console.WriteLine($"Please return the book before {returnDeadline}. Happy reading!");
        }

        public void AskToChooseShorterPeriod()
        {
            Console.WriteLine("You can't take book for longer than 2 months :( n/ Would you like to take book for 2 months? (y / n)");
            string answer = Console.ReadLine();
            if (answer == "y")
            {
                int period = 61;
                ConfirmBorrowingPeriodAvailability(period);
            }
            if (answer == "n")
            {
                Console.WriteLine("Please choose another period:");
                int period = Int32.Parse(Console.ReadLine());
                CheckBorrowingPeriodAvailability(period);
            }
        }
    }
}
