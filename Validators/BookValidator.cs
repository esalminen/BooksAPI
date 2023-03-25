using Books.Models;
using FluentValidation;

namespace BooksAPI.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Year).NotNull();
            RuleFor(x => x.Publisher).Must(NotEmptyOrNull);
        }
        
        private bool NotEmptyOrNull(string? value)
        {
            if (value == null || value.Length > 0)
            {
                return true;
            }
            return false;
        }
    }
}
