using FluentValidation;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class GetAllMovieOptionsValidator : AbstractValidator<GetAllMovieOptions>
    {
        private readonly string[] AcceptableSortValues =
        {
            "title",
            "yearofrelease",
        };

        public GetAllMovieOptionsValidator()
        {
            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.UtcNow.Year);

            RuleFor(x => x.SortField)
                .Must(x => x is null || AcceptableSortValues.Contains(x.ToLowerInvariant()))
                .WithMessage($"Sort field must be one of the following: {string.Join(", ", AcceptableSortValues)}.");
        }
    }
}
