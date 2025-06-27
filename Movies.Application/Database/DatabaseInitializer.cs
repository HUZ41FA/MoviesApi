using Dapper;

namespace Movies.Application.Database
{
    public class DatabaseInitializer
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }   

        public async Task InitializeAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS movies (
                    Id UUID PRIMARY KEY,
                    Title VARCHAR(255) NOT NULL,
                    Slug VARCHAR(250)  NOT NULL,
                    YearOfRelease INT NOT NULL)
                ");

            await connection.ExecuteAsync(@"
                CREATE UNIQUE INDEX CONCURRENTLY IF NOT EXISTS idx_movies_slug
                ON movies USING btree(Slug);
            ");

            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS genres (
                    movieId UUID references movies (Id),
                    name text NOT NULL)
                ");
        }
    }
}
