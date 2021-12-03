using BookLibraryBackend.Repository;
using BookLibraryBackend.Services;
using System;

namespace BookLibraryBackend
{
    class Program
    {
        static void Main()
        {
            BookService bookService = new(new BookRepository());
            BorrowingService borrowingService = new(new BookRepository(), bookService);

            Console.WriteLine(@"Commands:
                To list all books --> List books
                To filter books list --> Filter books list
                To add a new book --> Add book
                To delete a book --> Delete book
                To take a book --> Take book
                To return a book --> Return book
                To exit app --> Exit");
            string command = "";

            while (command != "Exit")
            {
                command = Console.ReadLine();
                if (command == "Add book")
                {
                    bookService.AddNewBook();
                }
                else if (command == "Delete book")
                {
                    bookService.DeleteBook();
                }
                else if (command == "List books")
                {
                    bookService.ListAllBooks();
                }
                else if (command == "Filter books list")
                {
                    bookService.FilterAndListBooks();
                }
                else if (command == "Take book")
                {
                    borrowingService.TakeBook();
                }
                else if (command == "Return book")
                {
                    borrowingService.ReturnBook();
                }
                else if (command == "Help")
                {
                    Console.WriteLine(@"Commands:
                    To list all books --> List books
                    To filter books list --> Filter books list
                    To add a new book --> Add book
                    To delete a book --> Delete book
                    To take a book --> Take book
                    To return a book --> Return book
                    To exit app --> Exit");
                }
                else
                {
                    Console.WriteLine("Wrong command was entered. Type Help to see all commands.");
                }
            }
        }
    }
}
