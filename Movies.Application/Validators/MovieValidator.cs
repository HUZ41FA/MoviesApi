using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        private readonly IMovieRepository _movieRepository;
        public MovieValidator(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Genres)
                .NotEmpty();

            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.UtcNow.Year);

            RuleFor(x => x.Slug)
                .MustAsync(ValidateSlug)
                .WithErrorCode("Movie already exists.");
        }

        public async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken cancellationToken)
        {
            var existingMovie = await _movieRepository.GetBySlugAsync(slug);

            if(existingMovie is not null)
            {
                return existingMovie.Id == movie.Id;
            }

            return existingMovie is null;
        }
    }
}
