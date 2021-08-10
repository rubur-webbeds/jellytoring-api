using Dapper;
using jellytoring_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Interests
{
    public class InterestsRepository : IInterestsRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public InterestsRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Interest>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryAsync<Interest>(InterestsQueries.GetAll);
        }
    }
}
