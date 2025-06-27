using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Database
{
    public class NpgConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public NpgConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnection()
        {
            var connection = new Npgsql.NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
