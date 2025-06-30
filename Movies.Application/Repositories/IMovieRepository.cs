using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public interface IMovieRepository
    {
        Task<bool> CreateAsync(Movie movie, CancellationToken token = default);
        Task<bool> UpdateAsync(Movie movie, CancellationToken token = default);
        Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);
        Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);
        Task<IEnumerable<Movie>> GetAllAsync(GetAllMovieOptions options, CancellationToken token = default); 
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    }
}
