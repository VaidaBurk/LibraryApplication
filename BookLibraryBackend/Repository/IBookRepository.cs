using BookLibraryBackend.Models;
using System;
using System.Collections.Generic;

namespace BookLibraryBackend.Repository
{
    public interface IBookRepository
    {
        public List<Book> ReadFileAndDeserialize(string filePath);

        public void WriteToFile(Object anyObject, string filePath);
    }
}
