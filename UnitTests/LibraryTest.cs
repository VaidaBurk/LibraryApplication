using BookLibraryBackend.Models;
using BookLibraryBackend.Repository;
using BookLibraryBackend.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetBooksTest()
        {
            // arrange
            var book = new Book()
            {
                Name = "testName",
                Author = "testAuthor",
                Category = "testCategory",
                Language = "testLang",
                PublicationDate = "1900",
                ISBN = "testIsbn12345",
                IsBookTaken = false,
                ReaderId = 1,
                ReturnDeadline = System.DateTime.Now,
            };
            List<Book> books = new() { book };

            Mock<IBookRepository> mockBookRepository = new();
            mockBookRepository.Setup(b => b.ReadFileAndDeserialize()).Returns(books);

            BookReader bookService = new(mockBookRepository.Object);

            // act
            List<Book> fetchedBooks = bookService.GetBooks();

            // assert
            Book fetchedBook = fetchedBooks.FirstOrDefault();
            Assert.AreEqual(book.Name, fetchedBook.Name);
            Assert.AreEqual(book.Author, fetchedBook.Author);
            Assert.AreEqual(book.Category, fetchedBook.Category);
            Assert.AreEqual(book.Language, fetchedBook.Language);
            Assert.AreEqual(book.PublicationDate, fetchedBook.PublicationDate);
            Assert.AreEqual(book.ISBN, fetchedBook.ISBN);
            Assert.AreEqual(book.IsBookTaken, fetchedBook.IsBookTaken);
            Assert.AreEqual(book.ReaderId, fetchedBook.ReaderId);
            Assert.AreEqual(book.ReturnDeadline, fetchedBook.ReturnDeadline);
        }

        //[Test]
        //public void GetBookByIsbnTest()
        //{
        //    //arrange
        //    var book = new Book()
        //    {
        //        Name = "testName",
        //        Author = "testAuthor",
        //        Category = "testCategory",
        //        Language = "testLang",
        //        PublicationDate = "1900",
        //        ISBN = "testIsbn12345",
        //        IsBookTaken = false,
        //        ReaderId = 1,
        //        ReturnDeadline = System.DateTime.Now,
        //    };
        //    List<Book> books = new() { book };

        //    string isbn = "0000000000000";
        //    Mock<IBookRepository> mockBookRepository = new();

        //    // act
        //    BookAction bookAction = new(mockBookRepository.Object, new BookReader(mockBookRepository.Object));
        //    Book fetchedBook = bookAction.GetBookByIsbn(isbn, books);

        //    //assert
        //    Assert.AreEqual(null, fetchedBook);
        //}
    }
}