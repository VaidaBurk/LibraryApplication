using BookLibraryBackend.Models;
using System;
using System.Collections.Generic;

namespace BookLibraryBackend.Repository
{
    public interface IBookRepository
    {
        public List<Book> ReadFileAndDeserialize();

        public void WriteToFile(Object anyObject);
    }
}
