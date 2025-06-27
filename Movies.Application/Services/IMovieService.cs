using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public interface IMovieService
    {
        Task<bool> CreateAsync(Movie movie);
        Task<Movie?> UpdateByIdAsync(Movie movie);
        Task<Movie?> GetByIdAsync(Guid id);
        Task<Movie?> GetBySlugAsync(string slug);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
