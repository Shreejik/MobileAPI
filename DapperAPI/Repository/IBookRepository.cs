namespace DapperAPI.Repository
{
    public interface IBookRepository
    {

        IEnumerable<Book> GetAllBooks();

        Book GetBook(int id);

        void DeleteBook(int id);

        Book AddBook (Book book);

        Book UpdateBook(Book book);

        Book GetBookByAuthor(string author);
    }
}
