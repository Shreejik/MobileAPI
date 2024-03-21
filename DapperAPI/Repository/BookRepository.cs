using Dapper;
using DapperAPI.Data;
using System.Data;

namespace DapperAPI.Repository
{
    public class BookRepository: IBookRepository
    {

        private readonly IDbConnection _connection;
        public BookRepository(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public Book AddBook(Book book)
        {
            //book.BookId = new();
            var sql = "INSERT INTO BOOK (BookId, BookTitle, BookPrice, BookAuthor, BookPublication) VALUES (@BookId, @BookTitle, @BookPrice, @BookAuthor, @BookPublication)";
            return _connection.QueryFirstOrDefault(sql, book);
        }

        public void DeleteBook(int id)
        {
             var sql = "DELETE FROM BOOK WHERE BookId = @BookId";
            _connection.Execute(sql, new {BookId = id });
        }

        public IEnumerable<Book> GetAllBooks()
        {
            //return _connection.Query<Book>("SELECT * FROM BOOK");
            return _connection.Query<Book>("SELECT * FROM BOOK");

        }

        public Book GetBook(int id)
        {
            return _connection.QuerySingleOrDefault<Book>("SELECT * FROM BOOK WHERE BookId = @BookId", new {BookId = id});
        }

        public Book GetBookByAuthor(string author)
        {
            return _connection.QuerySingleOrDefault<Book>("SELECT * FROM BOOK WHERE BookAuthor = @BookAuthor", new {BookAuthor = author});
        }

        public Book UpdateBook(Book book)
        {
            var sql = "UPDATE BOOK SET BookId = @BookId, BookTitle = @BookTitle, BookPrice = @BookPrice, BookAuthor = @BookAuthor, BookPublication = @BookPublication WHERE BookId = @BookId";
            return _connection.QueryFirstOrDefault(sql, book);
        }
    }
}
