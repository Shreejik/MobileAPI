using System.ComponentModel.DataAnnotations;

namespace DapperAPI
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        public string BookTitle { get; set; }

        public int BookPrice { get; set; }

        public string BookAuthor { get; set; }

        public string BookPublication { get; set; }
    }
}
