using Movies.Application.Models;
using Movies.Contract.Requests;
using Movies.Contract.Responses;

namespace Movies.Api.Mappings
{
    public static class ContractMapping
    {
        public static Movie MapToMovie(this CreateMovieRequest request)
        {
            return new Movie()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
        {
            return new Movie()
            {
                Id = id,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static MovieResponse MapToResponse(this Movie movie)
        {
            return new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title,
                Slug = movie.Slug,
                UserRating = movie.UserRating,
                Rating = movie.Rating,
                YearOfRelease = movie.YearOfRelease,
                Genres = movie.Genres
            };
        }

        public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies) 
        {
            return new MoviesResponse
            {
                Items = movies.Select(m => m.MapToResponse()).ToList()
            };
        }

        public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
        {
            return ratings.Select(x => new MovieRatingResponse
            {
                Rating = x.Rating,
                Slug = x.Slug,
                MovieId = x.MovieId
            });
        }

        public static GetAllMovieOptions ToMovieOptions(this GetAllMovieRequest request)
        {
            return new GetAllMovieOptions
            {
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                SortField = request.SortBy?.Trim('+', '-'),
                SortOrder = request.SortBy is null ? SortOrder.None :
                    request.SortBy.StartsWith('+') ? SortOrder.Ascending : SortOrder.Descending
            };
        }

        public static GetAllMovieOptions WithUser(this GetAllMovieOptions options, Guid? userId)
        {
            options.UserId = userId;
            return options;
        }
    }
}
