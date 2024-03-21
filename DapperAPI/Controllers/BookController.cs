using DapperAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService; 
        }

        [HttpGet]
        public IActionResult GetAllBooks() {

            return Ok("Working api");
        }

        [HttpGet("id")]

        public IActionResult GetBook(int id)
        {
            return Ok(_bookService.GetBook(id));
        }

        [HttpGet("{author}")]

        public IActionResult GetBookByAuthor(string author)
        {
            return Ok(_bookService.GetBookByAuthor(author));
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            
            return Ok(_bookService.AddBook(book));

        }

        [HttpDelete("id")]
        public IActionResult DeleteBook(int id)
        {
            var deleteBook = _bookService.GetBook(id);

            if(deleteBook != null)
            {
                _bookService.DeleteBook(id);
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("{id}")]

        public IActionResult UpdateBook(Book book, [FromRoute]int id) {

            var updateBook = _bookService.GetBook(id);

            if (updateBook != null)
            {
                _bookService.UpdateBook(updateBook);
                return Ok(updateBook);
            }
            return NotFound();

        }
    }
}
