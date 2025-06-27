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
        public Task<bool> CreateAsync(Movie movie)
        {
            return _movieRepository.CreateAsync(movie);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _movieRepository.DeleteByIdAsync(id);  
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return  _movieRepository.GetAllAsync();
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            return _movieRepository.GetByIdAsync(id);
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            return _movieRepository.GetBySlugAsync(slug);
        }

        public async Task<Movie?> UpdateByIdAsync(Movie movie)
        {
            var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id);

            if (!movieExists)
            {
                return null;
            }

            await _movieRepository.UpdateByIdAsync(movie);

            return movie;
        }
    }
}
