using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Application.Database;

namespace Movies.Api.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public const string Name = "Database";
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<DatabaseHealthCheck> _logger;

        public DatabaseHealthCheck(IDatabaseConnectionFactory connectionFactory, ILogger<DatabaseHealthCheck> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                _ = await _connectionFactory.CreateConnectionAsync();
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                string errorMessage = "Database is unhealthy";
                _logger.LogError(errorMessage, e);
                return HealthCheckResult.Unhealthy(errorMessage, e);
            }
        }
    }
}
