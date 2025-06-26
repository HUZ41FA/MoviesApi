using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new();

        public Task<bool> CreateAsync(Movie movie)
        {
            _movies.Add(movie);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedItems = _movies.RemoveAll(m => m.Id == id);
            var result = removedItems > 0;
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return Task.FromResult(_movies.AsEnumerable());
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if(movie == null)
            {
                return Task.FromResult<Movie?>(null);
            }

            return Task.FromResult<Movie?>(movie);
        }

        public Task<bool> UpdateByIdAsync(Movie movie)
        {
            var movieIndex = _movies.FindIndex(m => m.Id == movie.Id);
            if(movieIndex == -1)
            {
                return Task.FromResult(false);
            }

            _movies[movieIndex] = movie;
            return Task.FromResult(true);
        }
    }
}
