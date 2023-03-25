using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Books.Data;
using Books.Models;
using FluentValidation;
using FluentValidation.Results;

namespace BooksAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksContext _context;
        private readonly IValidator<Book> _validator;

        public BooksController(BooksContext context, IValidator<Book> validator)
        {
            _context = context;
            _validator = validator;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <param name="author">Optional search text for author, must not be an empty string</param>
        /// <param name="year">Optional search year, must be an integer</param>
        /// <param name="publisher">Optional search text for publisher, must not be an empty string</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /books
        ///
        /// </remarks>
        /// <response code="200">Returns a collection of books</response>
        /// <response code="400">Bad search parameters</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? author, int? year, string? publisher)
        {
            // Validate search parameters
            if (author != null && author.Length == 0) return BadRequest();
            if (publisher != null && publisher.Length == 0) return BadRequest();

            // Search and filter results with given parameters
            return await _context.Books
                .Where(b => (author == null || b.Author.ToLower().Equals(author.ToLower())) &&
                            (year == null || b.Year == year) &&
                            (publisher == null || b.Publisher.ToLower().Equals(publisher.ToLower())))
                .ToListAsync();
        }

        /// <summary>
        /// Get a book
        /// </summary>
        /// <param name="id">Book id</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /books/4
        ///
        /// </remarks>
        /// <response code="200">Returns a book</response>
        /// <response code="404">Book not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            // Handle non integer id
            if (!int.TryParse(id, out int bookId)) return NotFound();

            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// Post a book
        /// </summary>
        /// <param name="book">Book</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /books
        ///     {
        ///        "title":"My book",
        ///        "author":"John Doe",
        ///        "year":1998,
        ///        "publisher":"Otava",
        ///        "description":"Boring book about the life of John Doe"
        ///     }
        ///
        /// </remarks>
        /// <returns>Response statuscode</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response200),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            // Validate book model
            ValidationResult result = await _validator.ValidateAsync(book);
            if (!result.IsValid)
            {
                return BadRequest();
            }
            try
            { 
                _context.Books.Add(book);
                await _context.SaveChangesAsync();            
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return Ok(new { id = book.Id });
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id">Book id</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /book/4
        ///
        /// </remarks>
        /// <returns>Response statuscode</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(string id)
        {
            // Handle non integer id
            if (!int.TryParse(id, out int bookId)) return NotFound();

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete all books (TESTING PURPOSES ONLY!!! REMOVE FROM PRODUCTION CODE)
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public  IActionResult DeleteAllBooks()
        {
            foreach (var book in _context.Books)
                _context.Books.Remove(book);
            _context.SaveChanges();

            // Reset the auto-incrementing ID for Books table
            _context.Database.ExecuteSqlRaw("DELETE FROM Books");
            _context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='Books'");
            return NoContent();
        }
    }

    // Documenting classes for swagger response (terrible way to do this!)
    public class Response200
    {
        public int id { get; set; } = 4;
    }
}
