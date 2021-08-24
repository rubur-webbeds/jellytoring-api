using Dapper;
using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Statuses
{
    public class StatusesRepository : IStatusesRepository
    {
        private const string PendingStatusCode = "PEND";
        private const string ApprovedStatusCode = "APPR";
        private const string DiscardedStatusCode = "DISC";

        private readonly IConnectionFactory _connectionFactory;

        public StatusesRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryAsync<Status>(StatusesQueries.GetAll);
        }

        public Task<Status> GetPendingAsync() => GetAsync(PendingStatusCode);
        public Task<Status> GetApprovedAsync() => GetAsync(ApprovedStatusCode);
        public Task<Status> GetDiscardedAsync() => GetAsync(DiscardedStatusCode);

        private async Task<Status> GetAsync(string statusCode)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<Status>(StatusesQueries.Get, new { statusCode });
        }
    }
}
