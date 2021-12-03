using BookLibraryBackend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BookLibraryBackend.Repository
{
    public class BookRepository : IBookRepository
    {
        public virtual List<Book> ReadFileAndDeserialize(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Book>>(jsonData);
        }

        public virtual void WriteToFile(Object anyObject, string filePath)
        {
            string jsonString = JsonConvert.SerializeObject(anyObject);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
