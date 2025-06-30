using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    internal class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<Movie> _movieValidator;
        private readonly IValidator<GetAllMovieOptions> _movieOptionsValidator;
        public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator, IValidator<GetAllMovieOptions> movieOptionsValidator)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
            _movieOptionsValidator = movieOptionsValidator;
        }   

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie, token);
            return await _movieRepository.CreateAsync(movie, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.DeleteByIdAsync(id, token);  
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMovieOptions options, CancellationToken token = default)
        {
            await _movieOptionsValidator.ValidateAndThrowAsync(options, token);
            return  await _movieRepository.GetAllAsync(options, token);
        }

        public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync(id, userId, token);
        }

        public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetBySlugAsync(slug, userId, token);
        }

        public async Task<Movie?> UpdateByIdAsync(Movie movie, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie, token);
            var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, token);

            if (!movieExists)
            {
                return null;
            }

            await _movieRepository.UpdateAsync(movie, token);

            return movie;
        }
    }
}
