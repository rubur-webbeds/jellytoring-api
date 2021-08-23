using Dapper;
using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Statuses
{
    public class StatusesRepository : IStatusesRepository
    {
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
    }
}
