using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappings;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contract.Requests;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
        {
            var movie = request.MapToMovie();
            await _movieService.CreateAsync(movie);
            return CreatedAtAction(nameof(Get), new { idORSlug = movie.Id}, movie.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] string idORSlug)
        {
            var movie = Guid.TryParse(idORSlug, out var id)
                ? await _movieService.GetByIdAsync(id)
                : await _movieService.GetBySlugAsync(idORSlug);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            var movieResponse = movies.MapToResponse();
            return Ok(movieResponse);
        }

        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
        {
            var movie = request.MapToMovie(id);

            var updatedMovie = await _movieService.UpdateByIdAsync(movie);

            if (updatedMovie is null)
            {
                return NotFound();
            }
            
            var response = movie.MapToResponse();

            return Ok(response);
        }

        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _movieService.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
