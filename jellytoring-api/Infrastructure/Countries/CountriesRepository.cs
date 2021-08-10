using Dapper;
using jellytoring_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Countries
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public CountriesRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryAsync<Country>(CountriesQueries.GetAll);
        }
    }
}
