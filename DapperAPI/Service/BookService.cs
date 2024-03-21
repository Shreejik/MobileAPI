using DapperAPI.Repository;

namespace DapperAPI.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Book AddBook(Book book)
        {
            return _bookRepository.AddBook(book);
        }

        public void DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book GetBook(int id)
        {
            return _bookRepository.GetBook(id);
        }

        public Book GetBookByAuthor(string author)
        {
            return _bookRepository.GetBookByAuthor(author);
        }

        public Book UpdateBook(Book book)
        {
            return _bookRepository.UpdateBook(book);
        }
    }
}
