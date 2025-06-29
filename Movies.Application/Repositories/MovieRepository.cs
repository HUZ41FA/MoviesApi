using Dapper;
using Movies.Application.Database;
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
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public MovieRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(token);
            var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                INSERT INTO movies (id, title, slug, yearofrelease)
                VALUES (@Id, @Title, @Slug, @YearOfRelease)
                """, movie, cancellationToken: token));

            if (result > 0)
            {
                foreach (var genre in movie.Genres.ToList())
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        INSERT INTO genres (movieid, name)
                        VALUES (@MovieId, @Name)
                        """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
                }
            }

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM genres WHERE movieid = @id
                """, new { id }, cancellationToken: token));

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM movies WHERE id = @id
                """, new { id }, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(token);

            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                SELECT count(1) FROM movies WHERE id = @id
                """, new { id }, cancellationToken: token));
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync(new CommandDefinition("""
                SELECT m.*, string_agg(g.name, ',') AS genres 
                FROM movies m 
                LEFT JOIN genres g ON m.id = g.movieid
                GROUP BY id 
                """, cancellationToken: token));

            return result.Select(x => new Movie
            {
                Id = x.id,
                Title = x.title,
                YearOfRelease = x.yearofrelease,
                Genres = Enumerable.ToList(x.genres.Split(','))
            });
        }

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(token);

            var movie = await connection.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition("""
                SELECT * FROM movies WHERE Id = @Id
                """, new { Id = id }, cancellationToken: token));

            if (movie == null)
            {
                return null;
            }

            var genres = await connection.QueryFirstOrDefaultAsync<List<string>>(new CommandDefinition("""
                SELECT name FROM genres WHERE movieid = @Id
                """, new { Id = id }, cancellationToken: token));

            foreach (var genre in genres)
            {
                movie.Genres.Add(genre);
            }

            return movie;
        }

        public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(token);

            var movie = await connection.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition("""
                SELECT * FROM movies WHERE slug = @Slug
                """, new { Slug = slug }, cancellationToken: token));

            if (movie == null)
            {
                return null;
            }

            var genres = await connection.QueryFirstOrDefaultAsync<List<string>>(new CommandDefinition("""
                SELECT name FROM genres WHERE movieId = @Id
                """, new { Id = movie.Id }, cancellationToken: token));

            foreach (var genre in genres)
            {
                movie.Genres.Add(genre);
            }

            return movie;
        }

        public async Task<bool> UpdateByIdAsync(Movie movie, CancellationToken token = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM genres WHERE movieid = @id
                """, new { id = movie.Id }, cancellationToken: token));

            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT INTO genres (movieId, name) 
                    VALUES (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
            }

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                UPDATE movies 
                SET slug = @Slug, title = @Title, yearofrelease = @YearOfRelease 
                WHERE id = @Id
                """, movie, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }
    }
}
