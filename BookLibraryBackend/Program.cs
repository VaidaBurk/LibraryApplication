using BookLibraryBackend.Models;
using BookLibraryBackend.Repository;
using BookLibraryBackend.Services;
using System;

namespace BookLibraryBackend
{
    class Program
    {
        static void Main()
        {
            BookRepository bookRepository = new();
            BookReader bookReader = new(bookRepository);
            BookAction bookAction = new(bookRepository, bookReader);

            PrintCommandOptions();

            string command = "Idle command";
            while (command != Command.Exit)
            {
                command = Console.ReadLine();
                if (command.StartsWith(Command.AddBook))
                {
                    HandleAddBookCommand(command);
                }
                else if (command.StartsWith(Command.DeleteBook))
                {
                    HandleDeleteBookCommand(command);
                }
                else if (command == Command.ListBooks)
                {
                    bookReader.DisplayBooks();
                }
                else if (command.StartsWith(Command.FilterBooks))
                {
                    HandleFilter(command);
                }
                else if (command.StartsWith(Command.TakeBook))
                {
                    HandleTakeBookCommand(command);
                }
                else if (command.StartsWith(Command.ReturnBook))
                {
                    HandleReturnBookCommand(command);
                }
                else if (command == Command.Help)
                {
                    PrintCommandOptions();
                }
                else
                {
                    Console.WriteLine("Wrong command was entered. Type Help to see all commands.");
                }
            }


            // TODO: move handlers to separate class ?CommandHandler?
            //  Flow: Program => ?Commandhandler? => BookAction && BookReader
            //  With this Program will not know about Book services and repo
            void HandleFilter(string command)
            {
                String[] inputs = command.Split(" -");
                bool IsCommandOnly = (inputs[0] == Command.FilterBooks && inputs.Length == 1);
                bool IsCommandWithParameters = inputs[0] == Command.FilterBooks && inputs.Length == 3;
                if (IsCommandOnly)
                {
                    Console.WriteLine("Enter: Filter books -parameter -search text \n Filter parameters: a - author; c - category; l - language; i - ISBN; n - name; av - availability (av -true for available books, av -false for taken books)");
                }
                else if (IsCommandWithParameters)
                {
                    string parameter = inputs[1];
                    string searchString = inputs[2].ToLower();
                    bookReader.FilterAndListBooks(parameter, searchString);
                }
                else
                {
                    Console.WriteLine("Wrong command was entered.");
                }
            }

            void HandleTakeBookCommand(string command)
            {
                String[] inputs = command.Split(" -");
                bool IsCommandOnly = (inputs[0] == Command.TakeBook && inputs.Length == 1);
                bool IsCommandWithParameters = inputs[0] == Command.TakeBook && inputs.Length == 4;
                if (IsCommandOnly)
                {
                    Console.WriteLine("Enter: Take book -ISBN (13 numbers) -Reader Id -Borrowing period in days");
                }
                else if (IsCommandWithParameters)
                {
                    string isbn = inputs[1];
                    int readerId = Int32.Parse(inputs[2]);
                    int period = Int32.Parse(inputs[3]);
                    bookAction.TakeBook(isbn, readerId, period);
                }
                else
                {
                    Console.WriteLine("Wrong command was entered.");
                }
            }

            void HandleReturnBookCommand(string command)
            {
                String[] inputs = command.Split(" -");
                bool IsCommandOnly = (inputs[0] == Command.ReturnBook && inputs.Length == 1);
                bool IsCommandWithParameters = inputs[0] == Command.ReturnBook && inputs.Length == 3;
                if (IsCommandOnly)
                {
                    Console.WriteLine("Enter: Return book -ISBN (13 numbers) -Reader Id");
                }
                else if (IsCommandWithParameters)
                {
                    string isbn = inputs[1];
                    int readerId = Int32.Parse(inputs[2]);
                    bookAction.ReturnBook(isbn, readerId);
                }
                else
                {
                    Console.WriteLine("Wrong command was entered.");
                }
            }

            void HandleDeleteBookCommand(string command)
            {
                String[] inputs = command.Split(" -");
                bool IsCommandOnly = (inputs[0] == Command.DeleteBook && inputs.Length == 1);
                bool IsCommandWithParameters = inputs[0] == Command.DeleteBook && inputs.Length == 2;
                if (IsCommandOnly)
                {
                    Console.WriteLine("Enter: Delete book -ISBN (13 numbers)");
                }
                else if (IsCommandWithParameters)
                {
                    string isbn = inputs[1];
                    bookAction.DeleteBook(isbn);
                }
                else
                {
                    Console.WriteLine("Wrong command was entered.");
                }
            }

            void HandleAddBookCommand(string command)
            {
                String[] inputs = command.Split(" -");
                bool IsCommandOnly = (inputs[0] == Command.AddBook && inputs.Length == 1); // TODO: extract to private function and reuse
                bool IsCommandWithParameters = inputs[0] == Command.AddBook && inputs.Length == 7; // TODO: rename to ?allParamentersPresent?
                if (IsCommandOnly)
                {
                    Console.WriteLine("Enter: Add book -ISBN -Name -Author -Publication year -Category -Language");
                }
                else if (IsCommandWithParameters)
                {
                    // TODO: construct to new Book and pass as parameter
                    string isbn = inputs[1];
                    string name = inputs[2];
                    string author = inputs[3];
                    string publicationDate = inputs[4];
                    string category = inputs[5];
                    string language = inputs[6];

                    bookAction.AddNewBook(isbn, name, author, publicationDate, category, language);
                }
                else
                {
                    Console.WriteLine("Wrong command was entered.");
                }
            }
        }

        private static void PrintCommandOptions()
        {
            Console.WriteLine(@$"Commands:
            To list all books --> {Command.ListBooks}
            To filter books list --> {Command.FilterBooks}
            To add a new book --> {Command.AddBook}
            To delete a book --> {Command.DeleteBook}
            To take a book --> {Command.TakeBook}
            To return a book --> {Command.ReturnBook}
            To exit app --> {Command.Exit}");
        }
    }
}
