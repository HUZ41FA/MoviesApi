using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappings;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contract.Requests;
using Movies.Contract.Responses;

namespace Movies.Api.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [MapToApiVersion(1.0)]
        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPost(ApiEndpoints.Movies.Create)]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movie = request.MapToMovie();
            await _movieService.CreateAsync(movie, token);
            return CreatedAtAction(nameof(Get), new { idORSlug = movie.Id}, movie.MapToResponse());
        }

        [MapToApiVersion(1.0)]
        [AllowAnonymous]
        [HttpGet(ApiEndpoints.Movies.Get)]
        [ResponseCache(Duration = 30, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] string idORSlug, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movie = Guid.TryParse(idORSlug, out var id)
                ? await _movieService.GetByIdAsync(id, userId, token)
                : await _movieService.GetBySlugAsync(idORSlug, userId, token);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie.MapToResponse());
        }

        [MapToApiVersion(1.0)]
        [AllowAnonymous]
        [HttpGet(ApiEndpoints.Movies.GetAll)]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "title", "yearOfRelease", "sortBy", "pageNumber", "pageSize"}, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
        [ProducesResponseType(typeof(MoviesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var getAllMovieOptions = request.ToMovieOptions().WithUser(userId);
            var movies = await _movieService.GetAllAsync(getAllMovieOptions, token);
            var movieCount = await _movieService.GetCountAsync(request.Title, request.YearOfRelease, token);
            var movieResponse = movies.MapToResponse(getAllMovieOptions.PageNumber, getAllMovieOptions.PageSize, movieCount);
            return Ok(movieResponse);
        }

        [MapToApiVersion(1.0)]
        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPut(ApiEndpoints.Movies.Update)]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movie = request.MapToMovie(id);

            var updatedMovie = await _movieService.UpdateByIdAsync(movie, token);

            if (updatedMovie is null)
            {
                return NotFound();
            }
            
            var response = movie.MapToResponse();

            return Ok(response);
        }

        [MapToApiVersion(1.0)]
        [Authorize(AuthConstants.AdminPolicyName)]
        [HttpDelete(ApiEndpoints.Movies.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)   
        {
            var deleted = await _movieService.DeleteByIdAsync(id, token);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
