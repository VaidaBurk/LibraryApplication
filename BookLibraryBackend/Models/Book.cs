using System;

namespace BookLibraryBackend.Models
{
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string PublicationDate { get; set; }
        public string ISBN { get; set; }
        public bool IsBookTaken { get; set; } = false;
        public int? ReaderId { get; set; }
        public DateTime? ReturnDeadline { get; set; }
    }
}
