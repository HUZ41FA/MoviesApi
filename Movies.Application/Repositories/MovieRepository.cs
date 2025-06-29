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

        public async Task<bool> CreateAsync(Movie movie)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                INSERT INTO movies (id, title, slug, yearofrelease)
                VALUES (@Id, @Title, @Slug, @YearOfRelease)
                """, movie));

            if (result > 0)
            {
                foreach (var genre in movie.Genres.ToList())
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        INSERT INTO genres (movieid, name)
                        VALUES (@MovieId, @Name)
                        """, new { MovieId = movie.Id, Name = genre }));
                }
            }

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM genres WHERE movieid = @id
                """, new { id }));

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM movies WHERE id = @id
                """, new { id }));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                SELECT count(1) FROM movies WHERE id = @id
                """, new { id }));
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var result = await connection.QueryAsync(new CommandDefinition("""
                SELECT m.*, string_agg(g.name, ',') AS genres 
                FROM movies m 
                LEFT JOIN genres g ON m.id = g.movieid
                GROUP BY id 
                """));

            return result.Select(x => new Movie
            {
                Id = x.id,
                Title = x.title,
                YearOfRelease = x.yearofrelease,
                Genres = Enumerable.ToList(x.genres.Split(','))
            });
        }

        public async Task<Movie?> GetByIdAsync(Guid id)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();

            var movie = await connection.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition("""
                SELECT * FROM movies WHERE Id = @Id
                """, new { Id = id }));

            if (movie == null)
            {
                return null;
            }

            var genres = await connection.QueryFirstOrDefaultAsync<List<string>>(new CommandDefinition("""
                SELECT name FROM genres WHERE movieid = @Id
                """, new { Id = id }));

            foreach (var genre in genres)
            {
                movie.Genres.Add(genre);
            }

            return movie;
        }

        public async Task<Movie?> GetBySlugAsync(string slug)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();

            var movie = await connection.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition("""
                SELECT * FROM movies WHERE slug = @Slug
                """, new { Slug = slug }));

            if (movie == null)
            {
                return null;
            }

            var genres = await connection.QueryFirstOrDefaultAsync<List<string>>(new CommandDefinition("""
                SELECT name FROM genres WHERE movieId = @Id
                """, new { Id = movie.Id }));

            foreach (var genre in genres)
            {
                movie.Genres.Add(genre);
            }

            return movie;
        }

        public async Task<bool> UpdateByIdAsync(Movie movie)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM genres WHERE movieid = @id
                """, new { id = movie.Id }));

            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT INTO genres (movieId, name) 
                    VALUES (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }));
            }

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                UPDATE movies 
                SET slug = @Slug, title = @Title, yearofrelease = @YearOfRelease 
                WHERE id = @Id
                """, movie));

            transaction.Commit();
            return result > 0;
        }
    }
}
