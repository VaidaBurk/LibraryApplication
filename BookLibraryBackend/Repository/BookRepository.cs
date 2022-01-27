using BookLibraryBackend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BookLibraryBackend.Repository
{
    public class BookRepository : IBookRepository
    {
        private const string _filePath = "books.json";

        public BookRepository()
        {
            if (!File.Exists(_filePath))
            {
                var jsonFile = File.Create(_filePath);
                jsonFile.Close();
                List<Book> books = new();
                WriteToFile(books);
            }
        }

        public virtual List<Book> ReadFileAndDeserialize()
        {
            string jsonData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Book>>(jsonData);
        }

        public virtual void WriteToFile(Object anyObject)
        {
            string jsonString = JsonConvert.SerializeObject(anyObject);
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
